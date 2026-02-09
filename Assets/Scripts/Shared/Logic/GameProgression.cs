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
    public DialogueSystemScript DialogueSystemScript; // move to GameData?
    public FadeEffect FadeEffect;
    public string currentScene; // TODO: is this needed?
    public HashSet<string> complementedOneTimeEvents = new();

    [Header("UI")]
    public GameObject blackTransition;
    public GameObject cgTransition;
    public RectTransform blackTransitionRectTransform;

    [Header("LOGIC")]
    public bool transitioning = true;

    // Flags
    private Dictionary<string, bool> flags = new Dictionary<string, bool> {
        
    };

    // BGM
    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private List<AudioClip> audioClipsBGM = new();
    private int currentBGM = 5;

    // SFX
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private List<AudioClip> audioClipsSFX = new();

    void Awake()
    {
        GameData.escapeRoomNumber = debugEscapeRoomNumber;

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

        DialogueSystemScript = (DialogueSystemScript)FindInChildrenIncludingInactive<DialogueSystemScript>(GameObject.Find("Canvas"));

        // TODO: fill out as we go
        switch (currentScene)
        {
            case "Cutscene":
                // TODO : different for endings
                StartCoroutine(PlayBGM(0));
                break;
            case "VisualNovel":
                break;
            case "EscapeRoom0":
                StartCoroutine(PlayBGM(1));
                break;
            case "EscapeRoom1":
            case "EscapeRoom2":
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
        DialogueSystemScript.SetVisualNovelJSONFile(dialogue);
        DialogueSystemScript.enabled = true;
        DialogueSystemScript.gameObject.SetActive(true);
    }

    // Flag
    public void SceneTransition(string scene)
    {
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

    public void CheckFlagsSet()
    {
        // TODO FUTURE IMPLEMTATION, SUPER HARDCODED
        // if (GetFlag("TODO"))
        // {
        //     GameObject.Find("POST TODO")?.SetActive(false);
        // }   
    }

    // BGM
    public void CheckBGMChange()
    {
        // TODO maybe check based on location for music?
    }

    public IEnumerator PlayBGM(int index, float waitTime = 0.75f, GameObject gameObjectToDeactivate = null, float gameWaitTime = 0f, float fadeSpeed = 0.25f)
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

        yield return new WaitForSeconds(waitTime);

        if (gameObjectToDeactivate)
        {
            gameObjectToDeactivate.SetActive(false);
        }

        if (index != -1)
        {
            for (float t = 0; t < fadeSpeed; t += Time.deltaTime)
            {
                audioSourceBGM.volume = Mathf.Lerp(0, 1, t / fadeSpeed);
                yield return null;
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
    public void Fade(string type, bool cg = false, string cgName = "")
    {
        if (!string.IsNullOrEmpty(cgName))
        {
            print($"getting {cgName}");
            cgTransition.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Art/CG/{cgName}");
        }
        
        if (type.Equals("in")) FadeEffect.FadeIn(!cg ? blackTransition : cgTransition, 0.5f);
            else FadeEffect.FadeOut(!cg ? blackTransition : cgTransition, 0.5f);
    }

    public Component FindInChildrenIncludingInactive<T>(GameObject parent) where T : Component
    {
        return parent.GetComponentsInChildren<T>(true).FirstOrDefault();
    }

    private void GetFPS()
    {
        Debug.Log("FPS: " + (1.0f / Time.deltaTime));
    }
}
