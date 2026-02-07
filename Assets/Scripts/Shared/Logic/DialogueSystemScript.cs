using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemScript : MonoBehaviour
{
    [Header("[DATA]")]
    private TextAsset dialogueStructListJSON;
    [SerializeField] private List<DialogueStruct> dialogueStructList = new();

    [Header("[UI]")]
    [SerializeField] private Image dialogueBoxImage;
    [SerializeField] private Image speakerSpriteImage;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI dialogueTMP;
    [SerializeField] private TextMeshProUGUI narrationTMP;
    [SerializeField] private GameObject advanceDialogueButton;

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
    [SerializeField] private AudioClip voiceSample;
    private AudioSource voiceAudioSource;

    void Awake()
    {
        // UI
        dialogueBoxImage = transform.Find("DialogueBox").GetComponent<Image>();
        speakerSpriteImage = transform.Find("SpeakerSprite").GetComponent<Image>();
        nameTMP = transform.Find("Text/NameText").GetComponent<TextMeshProUGUI>();
        dialogueTMP = transform.Find("Text/DialogueText").GetComponent<TextMeshProUGUI>();
        narrationTMP = transform.parent.transform.Find("NarrationText").GetComponent<TextMeshProUGUI>();
        advanceDialogueButton = transform.parent.transform.Find("AdvanceDialogueButton").gameObject;

        // Audio
        voiceAudioSource = GetComponent<AudioSource>(); 
    }

    void OnEnable() 
    {
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

        if (disableAdvanceCoroutine != null) StopCoroutine(DisableAdvance());
        disableAdvanceCoroutine = StartCoroutine(DisableAdvance());

        automaticFadeComplete = false;

        if (!currentDialogue.hideUI)
        {
            ShowUI();

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
            SetFlag();
        }

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
        dialogueBoxImage.enabled = true;
        speakerSpriteImage.enabled = true;
        nameTMP.enabled = true;
        dialogueTMP.enabled = true;
    }

    public void HideUI()
    {
        dialogueBoxImage.enabled = false;
        speakerSpriteImage.enabled = false;
        nameTMP.enabled = false;
        dialogueTMP.enabled = false;
    }
    
    public void SetSprite()
    {
        speakerSpriteImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[currentDialogue.speakerSprite];
    }

    public void SetDialogue() 
    {
        dialogueOnDisplay = currentDialogue.dialogue;

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
            
        switch (character)
        {
            case "Virgo":
                voiceAudioSource.pitch = 1.4f;
                break;
            case "4574":
            case "ASTA":
                voiceAudioSource.pitch = 1.15f;
                break;
            default:
                voiceAudioSource.pitch = 1.1f;
                break;
        }

        for (int i = 0; i <= dialogue.Length; i++)
        {
            tmp.text = dialogue[..i];

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
            print("this is cg");
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
    
    private void SetFlag()
    {
        if (!string.IsNullOrEmpty(currentDialogue.flag)) GameProgression.GameProgressionInstance.SceneTransition(currentDialogue.flag);
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
