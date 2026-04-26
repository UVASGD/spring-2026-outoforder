using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProgression : MonoBehaviour
{
    [Header("DEBUG")]
    [SerializeField] private int debugEscapeRoomNumber;

    [Header("DATA")]
    public static GameProgression GameProgressionInstance;
    public SpriteCache SpriteCache; // move to GameData?
    public DialogueSystem DialogueSystem; // move to GameData?
    public FadeEffect FadeEffect;
    public string currentScene; // TODO: is this needed?
    public HashSet<string> complementedOneTimeEvents = new();
    public List<string> hiddenInteractions = new()
    {
        { "hiddenInteractionManual" },
        { "hiddenInteractionPapers" },
        { "hiddenInteractionMirror" },
        { "hiddenInteractionEscapeRoom2A" },
        { "hiddenInteractionEscapeRoom2B" }
    };

    [Header("UI")]
    public GameObject blackTransition;
    public GameObject cgTransition;
    public RectTransform blackTransitionRectTransform;

    [Header("LOGIC")]
    public bool transitioning = true;

    // Flags
    private Dictionary<string, bool> flags = new();

    // BGM
    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private List<AudioClip> audioClipsBGM = new();
    private int currentBGM = -1;

    // SFX
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private List<AudioClip> audioClipsSFX = new();

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (GameProgressionInstance == null)
        {
            GameProgressionInstance = this;

            DontDestroyOnLoad(gameObject);

            FadeEffect = GetComponent<FadeEffect>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        GameData.escapeRoomNumber = (GameData.loadedSave != -1)
            ? PlayerPrefs.GetInt($"{GameData.loadedSave}_escapeRoomNumber")
            : debugEscapeRoomNumber;

        audioSourceBGM = GetComponent<AudioSource>();
        audioSourceSFX = transform.GetChild(0).GetComponent<AudioSource>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;

        StartCoroutine(SceneSetup());
    }

    private IEnumerator SceneSetup()
    {
        GameData.fadeCoroutine = null;

        DialogueSystem = (DialogueSystem)FindInChildrenIncludingInactive<DialogueSystem>(GameObject.Find("Canvas"));

        audioSourceBGM.loop = true;
        GameData.autoDialogueProgression = false;

        // TODO: fill out as we go
        switch (currentScene)
        {
            case "StartScreen":
                StartCoroutine(PlayBGM(0));
                break;
            case "Cutscene":
                // TODO : different for endings
                StartCoroutine(PlayBGM(1));
                audioSourceBGM.loop = false;
                GameData.autoDialogueProgression = true;
                break;
            case "VisualNovel":
                break;
            case "EscapeRoom0":
                StartCoroutine(PlayBGM(2));
                break;
            case "EscapeRoom1":
                StartCoroutine(PlayBGM(4));
                break;
            case "EscapeRoom2":
                StartCoroutine(PlayBGM(4));
                break;
            case "EscapeRoom3":
                break;
        }

        yield return new WaitForSeconds(0.25f);

        blackTransition = GameObject.Find("Canvas").transform.Find("BlackTransition").gameObject;

        float fadeSpeed = 0.5f;

        FadeEffect.FadeOut(blackTransition, fadeSpeed, transitioning: true);
    }

    // Dialogue
    public void ShowDialogue(TextAsset dialogue)
    {
        DialogueSystem.SetVisualNovelJSONFile(dialogue);
        DialogueSystem.enabled = true;
        DialogueSystem.gameObject.SetActive(true);
    }

    // Flag
    public void SceneTransition(string scene)
    {
        if (currentScene.Equals("StartScreen")) PlaySFX(0);

        transitioning = true;

        FadeEffect.FadeIn(blackTransition, fadeTime: 2f, scene: scene);
    }

    public bool GetFlag(string key)
    {
        return flags.TryGetValue(key, out bool value) && value;
    }

    public void SetFlag(string key, bool value)
    {
        flags[key] = value;
        Debug.Log($"SetFlag: {key} = {value}");
    }

    // BGM
    public void CheckBGMChange()
    {
        // TODO maybe check based on location for music?
    }

    public IEnumerator PlayBGM(int index, float waitTime = 0.75f, GameObject gameObjectToDeactivate = null, float gameWaitTime = 0f, float fadeSpeed = 0.25f, bool immediateStart = false)
    {
        print($"switching to music at index {index}");
        
        float startVolume = audioSourceBGM.volume;

        if (index == -1) fadeSpeed = 1.75f;
        if (index == -2) fadeSpeed = 5f;

        for (float t = 0; t < fadeSpeed; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(startVolume, 0, t / fadeSpeed);
            if (index == -2 && audioSourceBGM.volume <= 0.05f)
            {
                yield break;
            }
            yield return null;
        }

        audioSourceBGM.volume = 0;
        audioSourceBGM.Stop();

        yield return new WaitForSeconds(!immediateStart ? waitTime : 0f);

        if (gameObjectToDeactivate)
        {
            gameObjectToDeactivate.SetActive(false);
        }

        if (index != -1)
        {
            if (!immediateStart)
            {
                for (float t = 0; t < fadeSpeed; t += Time.deltaTime)
                {
                    audioSourceBGM.volume = Mathf.Lerp(0, 1, t / fadeSpeed);
                    yield return null;
                }
            }

            audioSourceBGM.volume = 1f;

            if (index == 5 || index != currentBGM)
            {
                audioSourceBGM.clip = audioClipsBGM[index];

                currentBGM = index;
            }

            audioSourceBGM.Play();

            yield return new WaitForSeconds(gameWaitTime);
        }
    }

    // SFX
    public void PlaySFX(int index)
    {
        audioSourceSFX.PlayOneShot(audioClipsSFX[index]);
    }

    // Fade
    public void Fade(string type, bool cg = false, string cgName = "", GameObject ui = null, float speed = 0.25f)
    {
        // TODO NEED TO CLEAN
        if (ui != null)
        {
            if (type.Equals("in")) FadeEffect.FadeIn(ui, speed);
            else FadeEffect.FadeOut(ui, speed);
            return;    
        }

        if (!string.IsNullOrEmpty(cgName))
        {
            print($"getting {cgName}");
            cgTransition.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Art/CG/{cgName}");
        }
        
        if (type.Equals("in")) FadeEffect.FadeIn(!cg ? blackTransition : cgTransition, 0.5f);
        else FadeEffect.FadeOut(!cg ? blackTransition : cgTransition, 0.5f);
    }

    // Other - OUTDATED

    public Component FindInChildrenIncludingInactive<T>(GameObject parent) where T : Component
    {
        return parent.GetComponentsInChildren<T>(true).FirstOrDefault();
    }

    // Save/Load
    [Serializable]
    public class FlagsData
    {
        public List<string> keys = new List<string>();
        public List<bool> values = new List<bool>();
    }
    
    public void Save(int saveToSlotNumber)
    {
        PlayerPrefs.SetInt($"{saveToSlotNumber}_escapeRoomNumber", GameData.escapeRoomNumber);

        PlayerPrefs.SetInt($"{saveToSlotNumber}_dialogueIndex", DialogueSystem.dialogueIndex - 1);

        PlayerPrefs.SetString($"{saveToSlotNumber}_locationTMP", DialogueSystem.locationTMP.text);

        PlayerPrefs.SetString($"{saveToSlotNumber}_speakerSpriteAstaImageActive", DialogueSystem.speakerSpriteAstaImageActive.sprite.name);
        PlayerPrefs.SetString($"{saveToSlotNumber}_speakerSpriteAstaImageOld", DialogueSystem.speakerSpriteAstaImageOld.sprite.name);
        PlayerPrefs.SetString($"{saveToSlotNumber}_speakerSpriteVirgoImageActive", DialogueSystem.speakerSpriteVirgoImageActive.sprite.name);
        PlayerPrefs.SetString($"{saveToSlotNumber}_speakerSpriteVirgoImageOld", DialogueSystem.speakerSpriteVirgoImageOld.sprite.name);

        PlayerPrefs.SetInt($"{saveToSlotNumber}_currentBGM", currentBGM);
        
        FlagsData wrapper = new();
        
        foreach (var kvp in flags)
        {
            wrapper.keys.Add(kvp.Key);
            wrapper.values.Add(kvp.Value);
        }

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString($"{saveToSlotNumber}_flags", json);
        
        PlayerPrefs.Save();

        print($"done saving to {saveToSlotNumber}");
    }

    public void Load(int loadFromSlotNumber)
    {
        GameData.escapeRoomNumber = PlayerPrefs.GetInt($"{loadFromSlotNumber}_escapeRoomNumber");
        
        string json = PlayerPrefs.GetString($"{loadFromSlotNumber}_flags");
        FlagsData wrapper = JsonUtility.FromJson<FlagsData>(json);
        
        Dictionary<string, bool> restoredFlags = new Dictionary<string, bool>();
        for (int i = 0; i < wrapper.keys.Count; i++)
        {
            restoredFlags.Add(wrapper.keys[i], wrapper.values[i]);
        }
        
        flags = restoredFlags;

        SceneTransition("VisualNovel");

        GameData.loadedSave = loadFromSlotNumber;
    }
}
