using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("[DATA]")]
    private TextAsset dialogueStructListJSON;
    [SerializeField] private List<DialogueStruct> dialogueStructList = new();

    [Header("[UI]")]
    private Image dialogueBoxImage;
    private GameObject oldCG;
    private GameObject activeCG;
    private Image oldCGImage;
    private Image activeCGImage;
    private Image speakerSpriteImage;
    private Image speakerSpriteAstaImage;
    private Image speakerSpriteVirgoImage;
    private TextMeshProUGUI nameTMP;
    private TextMeshProUGUI dialogueTMP;
    TextMeshProUGUI narrationTMP;
    private GameObject advanceDialogueButton;

    [Header("[LOGIC]")]
    public bool advanceDialogueButtonPressed;
    public bool transitioningScene;
    public bool finishedDialogue;
    public bool advanceDisabled;
    private Coroutine disableAdvanceCoroutine;
    private DialogueStruct currentDialogue;
    [SerializeField] private bool typeWriterInEffect;
    private Coroutine typewriterCoroutine;
    private int dialogueIndex = -1;
    private string dialogueOnDisplay;

    // FADE
    private bool automaticFadeComplete;
    private Coroutine automaticFadeCoroutine;

    [Header("[VOICES]")]
    [SerializeField] private List<AudioClip> voiceSamples;
    private AudioSource voiceAudioSource;

    void Awake()
    {
        // UI
        dialogueBoxImage = transform.Find("DialogueBox").GetComponent<Image>();
        oldCG = transform.Find("OldCG")?.gameObject;
        activeCG = transform.Find("ActiveCG")?.gameObject;
        oldCGImage = oldCG?.GetComponent<Image>();
        activeCGImage = activeCG?.GetComponent<Image>();
        speakerSpriteImage = transform.Find("SpeakerSprite")?.GetComponent<Image>();
        speakerSpriteAstaImage = transform.Find("SpeakerSpriteAsta")?.GetComponent<Image>();
        speakerSpriteVirgoImage = transform.Find("SpeakerSpriteVirgo")?.GetComponent<Image>();
        nameTMP = transform.Find("Text/NameText").GetComponent<TextMeshProUGUI>();
        dialogueTMP = transform.Find("Text/DialogueText").GetComponent<TextMeshProUGUI>();
        narrationTMP = transform.parent.transform.Find("NarrationText").GetComponent<TextMeshProUGUI>();
        advanceDialogueButton = transform.parent.transform.Find("AdvanceDialogueButton").gameObject;

        // Audio
        voiceAudioSource = GetComponent<AudioSource>(); 
    }

    void OnEnable() 
    {
        if (!SceneManager.GetActiveScene().name.Equals("Cutscene"))
        {
            oldCG?.gameObject.SetActive(false);
            activeCG?.gameObject.SetActive(false);
        }
        narrationTMP.gameObject.SetActive(false);
        advanceDialogueButton.SetActive(true);

        LoadVisualNovelJSONFile();
        ProgressMainVNSequence();

        GameData.currentlyTalking = true;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || advanceDialogueButtonPressed || automaticFadeComplete) && !typeWriterInEffect && automaticFadeCoroutine == null && !advanceDisabled)
        {
            advanceDialogueButtonPressed = false;

            if (currentDialogue.endOfScene)
            {
                advanceDialogueButton.SetActive(false);

                GameData.currentlyTalking = false;
                finishedDialogue = false;
                dialogueIndex = -1;
                enabled = false;

                speakerSpriteImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["Transparent"];
                gameObject.SetActive(false);

                if (GameData.escapeRoomGameplayManager != null) GameData.escapeRoomGameplayManager.interactingWith = "";
            }
            else if (!currentDialogue.endOfScene && !typeWriterInEffect && !finishedDialogue)
            {
                ProgressMainVNSequence();
            }
        }
        else if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || advanceDialogueButtonPressed) && typeWriterInEffect && currentDialogue.narration == null && !advanceDisabled)
        {
            advanceDialogueButtonPressed = false;
            
            SkipTypeWriterEffect();
        }
    }

    public void AdvanceDialogueButtonPressed()
    {
        advanceDialogueButtonPressed = true;
    }

    public void SetVisualNovelJSONFile(TextAsset characterDialogue)
    {
        dialogueStructListJSON = characterDialogue;
    }

    void LoadVisualNovelJSONFile()
    {
        // Data
        dialogueStructList = JsonConvert.DeserializeObject<DialogueStructListContainer>(dialogueStructListJSON.text).dialogues;
    }

    void ProgressMainVNSequence() 
    {
        dialogueIndex++;

        currentDialogue = dialogueStructList[dialogueIndex];

        if (currentDialogue.wait != 0)
        {
            StartCoroutine(SetWait(currentDialogue.wait));
        }
        else
        {
            if (disableAdvanceCoroutine != null) StopCoroutine(DisableAdvance());
            disableAdvanceCoroutine = StartCoroutine(DisableAdvance());

            automaticFadeComplete = false;
            
            if (!currentDialogue.hideUI)
            {
                ShowUI();

                // TODO: do some sort of one other the other thing here
                // set cg
                SetCG();

                // set sprite
                SetSprite();

                // set dialogue
                SetDialogue();
            }
            else
            {
                HideUI();

                // set Narration (if any)
                SetNarration();

                // set Fade (if any)
                SetFade();

                // set Flag (if any)
                SetScene();

                // set Action (if any)
                SetMethod();
            }
        }

        // set flag (if any)
        SetFlag();

        // set BGM (if any)
        SetBGM();

        // set SFX (if any)
        SetSFX();

        if (dialogueIndex == dialogueStructList.Count - 1)
        {
            finishedDialogue = true;
        }
    }

    public void ShowUI()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Cutscene"))
        {
            dialogueBoxImage.enabled = true;
            if (SceneManager.GetActiveScene().name.Contains("EscapeRoom"))
            {
                speakerSpriteImage.enabled = true;
            }
            else if (SceneManager.GetActiveScene().name.Equals("VisualNovel"))
            {
                speakerSpriteAstaImage.enabled = true;
                speakerSpriteVirgoImage.enabled = true;
            }
        }
        nameTMP.enabled = true;
        dialogueTMP.enabled = true;
    }

    public void HideUI()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Cutscene"))
        {
            dialogueBoxImage.enabled = false;
            if (SceneManager.GetActiveScene().name.Contains("EscapeRoom"))
            {
                speakerSpriteImage.enabled = false;
            }
            else if (SceneManager.GetActiveScene().name.Equals("VisualNovel"))
            {
                speakerSpriteAstaImage.enabled = false;
                speakerSpriteVirgoImage.enabled = false;
            }
        }
        nameTMP.enabled = false;
        dialogueTMP.enabled = false;
    }

    private IEnumerator SetWait(float waitTime)
    {
        HideUI();
        advanceDisabled = true;

        yield return new WaitForSeconds(waitTime);

        advanceDisabled = false;
        advanceDialogueButtonPressed = true;
        ShowUI();
    }

    public void SetCG()
    {
        if (!SceneManager.GetActiveScene().name.Contains("EscapeRoom"))
        {
            if (currentDialogue.cgSprite != null)
            {
                Sprite oldCGSprite = oldCGImage.sprite;
                Sprite newCGSprite =  GameProgression.GameProgressionInstance.SpriteCache.sprites[currentDialogue.cgSprite];

                if (!oldCGSprite.ToString().Equals(newCGSprite.ToString())) 
                {
                    oldCGImage.sprite = activeCGImage.sprite;
                    GameProgression.GameProgressionInstance.FadeEffect.FadeInCGSprite(activeCG, newCGSprite);
                    GameProgression.GameProgressionInstance.FadeEffect.FadeOutCGSprite(oldCG, oldCGSprite);
                }
            }    
        }
    }
    
    public void SetSprite()
    {
        if (SceneManager.GetActiveScene().name.Contains("EscapeRoom"))
        {
            if (currentDialogue.speakerSprite != null) speakerSpriteImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[currentDialogue.speakerSprite];
        }
        else if (SceneManager.GetActiveScene().name.Equals("VisualNovel"))
        {
            if (currentDialogue.speakerSpriteAsta != null) speakerSpriteAstaImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[currentDialogue.speakerSpriteAsta];
            speakerSpriteAstaImage.color = currentDialogue.character.Equals("???") || currentDialogue.character.Contains("4574") || currentDialogue.character.Equals("ASTA") 
                ? Color.white
                : Color.gray;

            if (currentDialogue.speakerSpriteVirgo != null) speakerSpriteVirgoImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[currentDialogue.speakerSpriteVirgo];
            speakerSpriteVirgoImage.color = currentDialogue.character.Equals("Virgo")
                ? Color.white
                : Color.gray;
        }
    }

    public void SetDialogue() 
    {
        dialogueOnDisplay = currentDialogue.dialogue;

        if (!GameProgression.GameProgressionInstance.currentScene.Equals("Cutscene"))
        {
            // set dialogue box image color
            switch (currentDialogue.character)
            {
                case "Virgo":
                    dialogueBoxImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["TextboxVirgo"];
                    break;
                case "4574":
                case "ASTA":
                    dialogueBoxImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["TextboxAsta"];
                    break;
                default:
                    dialogueBoxImage.sprite =  GameProgression.GameProgressionInstance.SpriteCache.sprites["Textbox"];
                    break;
            }
        }        

        // set character name
        nameTMP.text = currentDialogue.character;

        // set dialogue
        typewriterCoroutine = StartCoroutine(TypewriterEffect(currentDialogue.character, currentDialogue.dialogue));
    }

    void SkipTypeWriterEffect() 
    {
        StopCoroutine(typewriterCoroutine);
        dialogueTMP.text = dialogueOnDisplay;
        typeWriterInEffect = false;
    }

    float GetTextSpeed() 
    {
        if (currentDialogue.textSpeed != 0f)
        {
            return currentDialogue.textSpeed; // modified | can also be different for different CharacterEnum
        }
        else if (currentDialogue.narration != null)
        {
            return 0.02f;
        }
        return 0.005f; // default
    }

    private IEnumerator TypewriterEffect(string character, string dialogue, bool narration = false, bool end = false)
    {
        typeWriterInEffect = true;

        TextMeshProUGUI tmp = !narration
            ? dialogueTMP
            : narrationTMP;

        if (narration)
        {
            if (end)
            {
                tmp.text = "";
                yield return new WaitForSeconds(1f);
                tmp.alignment = TextAlignmentOptions.Left;
                tmp.rectTransform.anchoredPosition = new Vector2(0f, -250f);
            }
            else if (currentDialogue.cg)
            {
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.rectTransform.anchoredPosition = new Vector2(0f, -250f);
            }
            else
            {
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.rectTransform.anchoredPosition = new Vector2(0f, 0f);
            }
        }
       
        float textSpeed = !end
            ? GetTextSpeed()
            : 0.05f;

        AudioClip voiceSample = voiceSamples[0];
            
        switch (character)
        {
            case "Virgo":
                voiceAudioSource.pitch = 1.4f;
                voiceSample = voiceSamples[0];
                break;
            case "???":
            case "4574?":
                voiceAudioSource.pitch = 0.85f;
                break;
            case "4574":
            case "ASTA":
                voiceAudioSource.pitch = 0.95f;
                break;
            default:
                voiceAudioSource.pitch = 1.1f;
                break;
        }

        for (int i = 0; i <= dialogue.Length; i++)
        {
            tmp.text = dialogue[..i];

            if (character.Equals("???") || character.Equals("4574?")) voiceSample = voiceSamples[Random.Range(4, 7)];
            if (character.Equals("4574") || character.Equals("ASTA")) voiceSample = voiceSamples[Random.Range(1, 4)];
            if (!dialogue.Equals("(...)")) voiceAudioSource.PlayOneShot(voiceSample);

            yield return new WaitForSeconds(textSpeed); // make diff speeds
        }

        typeWriterInEffect = false;

        if (narration)
        {
            yield return new WaitForSeconds(0.5f);
        }

        advanceDisabled = false;
    }

    private void SetFlag()
    {
        if (currentDialogue.flag != null) GameProgression.GameProgressionInstance.SetFlag(currentDialogue.flag, true);
    }

    private void SetBGM()
    {
        if (currentDialogue.playBGM != null) StartCoroutine(GameProgression.GameProgressionInstance.PlayBGM(int.Parse(currentDialogue.playBGM)));
    }

    private void SetSFX()
    {
        if (currentDialogue.playSFX != null) GameProgression.GameProgressionInstance.PlaySFX(int.Parse(currentDialogue.playSFX));
    }

    public void SetNarration()
    {
        if (currentDialogue.narration != null)
        {
            narrationTMP.enabled |= true;

            string[] narrationType = currentDialogue.narration.Split("|");

            if (narrationType.Length < 2) narrationType = new[] { narrationType[0], "" };

            dialogueOnDisplay = currentDialogue.narration;

            // set narration
            if (narrationType[1].Equals("end"))
            {
                StartCoroutine(TypewriterEffect("", narrationType[0], narration: true, end: true));

            }
            else if (!string.IsNullOrEmpty(narrationType[1]))
            {
                typewriterCoroutine = StartCoroutine(TypewriterEffect(narrationType[1], narrationType[0], narration: true));
            }
            else
            {
                typewriterCoroutine = StartCoroutine(TypewriterEffect("", currentDialogue.narration, narration: true));
            }
        }
        else
        {
            narrationTMP.enabled &= false;
        }
    }

    public void SetFade()
    {
        if (currentDialogue.fade != null) automaticFadeCoroutine = StartCoroutine(AutomaticFade());
    }

    private IEnumerator AutomaticFade()
    {
        string[] fadeType = currentDialogue.fade.Split(",");

        if (fadeType.Length < 2) fadeType = new[] { fadeType[0], "" };

        bool cg = false;

        // TODO: change this for what the cgs are called
        if (fadeType[1].Equals("HandHold") || fadeType[1].Equals("Hug"))
        {
            cg = true;
        }
        else if (!string.IsNullOrEmpty(fadeType[1]))
        {
            GameProgression.GameProgressionInstance.blackTransitionRectTransform.sizeDelta = new Vector2(GameProgression.GameProgressionInstance.blackTransitionRectTransform.sizeDelta.x, 120);
        }   

        GameProgression.GameProgressionInstance.Fade(fadeType[0], cg, cg? fadeType[1] : "");

        yield return new WaitForSeconds(1.25f);
            
        advanceDisabled = false;
        automaticFadeComplete = true;

        automaticFadeCoroutine = null;
    }
    
    private void SetScene()
    {
        if (!string.IsNullOrEmpty(currentDialogue.scene)) GameProgression.GameProgressionInstance.SceneTransition(currentDialogue.scene);
    }

    private void SetMethod()
    {
        if (!string.IsNullOrEmpty(currentDialogue.method)) 
        {
            string[] method = currentDialogue.method.Split(',');
            if (method.Length > 1) 
            {
                GameData.escapeRoomGameplayManager.locations.SendMessage(method[0], method[1]);
            }
            else 
            {
                GameData.escapeRoomGameplayManager.locations.SendMessage(method[0]);        
            }
        }
    }

    private IEnumerator DisableAdvance()
    {
        advanceDisabled = true;

        if (currentDialogue.fade == null && currentDialogue.narration == null)
        {
            yield return new WaitForSeconds(0.60f);

            advanceDisabled = false;
        }

        disableAdvanceCoroutine = null;
    }
}
