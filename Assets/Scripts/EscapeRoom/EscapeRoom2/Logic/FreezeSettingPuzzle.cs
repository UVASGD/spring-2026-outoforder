using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FreezeSettingPuzzle : Puzzle
{
    // TODO -- MAKE ALL PRIVATE
    public GameObject slider;
    public Image mask;
    public Slider sliderS;

    void Awake()
    {
        answer = new char[] { '1', '5' };
        guess = new char[] { '0', '0' };

        slider.SetActive(false);
        sliderS.onValueChanged.AddListener(delegate { ValueChangeCheck();});

        var tempColor = mask.color;
        tempColor.a = 1f;
        mask.color = tempColor;
        SetImageAlpha(0f);
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["FreezeSettingPuzzleSecondary"];

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionFreezeSettingPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedFreezeSettingPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
   
    public void SpawnSlider()
    {
        slider.SetActive(!slider.activeSelf);
    }

    public void ValueChangeCheck()
    {
        SetImageAlpha(sliderS.value);
    }

    public void SetImageAlpha(float alphaValue)
    {
        Color tempColor = mask.color;
        tempColor.a = alphaValue;
        mask.color = tempColor;
    }
}




