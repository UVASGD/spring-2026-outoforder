using System.Collections; // IEnumerator
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq; // Lets solve function access answer & guess outside of awake


public class FreezeSettingPuzzle : Puzzle
{    public GameObject submit;
    private bool leverRepaired;
    public GameObject slider;
    public Image mask;
    public Slider sliderS;
    public GameObject errorText;
    public float errorDuration = 2f;


    void Awake()
    {
        answer = new char[] { '1', '5' };
        guess = new char[] { '0', '0' };


        submit = transform.Find("SubmitButton").gameObject;
        slider.SetActive(false);
        errorText.SetActive(false);
        sliderS.onValueChanged.AddListener(delegate { ValueChangeCheck();});
        var tempColor = mask.color;
        tempColor.a = 1f;
        mask.color = tempColor;
        SetImageAlpha(0f); // Start transparent
    }


    void Update()
    {
        if (!leverRepaired && GameProgression.GameProgressionInstance.GetFlag("usedLever"))
        {
            leverRepaired = true;
            submit.GetComponent<Button>().enabled = true;
            submit.GetComponent<Image>().enabled = true;
        }
    }


    protected override void SolvedPuzzleSpecific()
    {
        //GameData.escapeRoomGameplayManagerScript.locations.ForEach(location => location.GetComponent<Image>().color = Color.white);


        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["UnlockedFreezeSettingPuzzle"];


        GameProgression.GameProgressionInstance.SetFlag("firstInteractionFreezeSettingPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedFreezeSettingPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
        GameProgression.GameProgressionInstance.SetFlag("lastInteractionFreezeSettingPuzzle", true);
    }
   
    public void spawnSlider()
    {
        if (slider.activeInHierarchy)
        {
            slider.SetActive(false);
        } else
        {
            slider.SetActive(true);
        }
    }


    public void SolvedFeedback()
    {
        if (!solved && answer.SequenceEqual(guess))
        {
            Debug.Log("Correct Answer");
        }
        else
        {
            StartCoroutine(ShowErrorMessage());
        }
    }


    private IEnumerator ShowErrorMessage()
    {
        errorText.SetActive(true);
        yield return new WaitForSeconds(errorDuration);
        errorText.SetActive(false);
    }

    public void ValueChangeCheck()
    {
        SetImageAlpha(sliderS.value);
    }

    public void SetImageAlpha(float alphaValue)
    {
        if (mask != null)
        {
            Color tempColor = mask.color; // Get the current color struct
            tempColor.a = alphaValue; // Modify the alpha component
            mask.color = tempColor; // Reassign the modified color back to the image
        }
    }
}




