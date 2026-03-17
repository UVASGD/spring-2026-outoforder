using UnityEngine;
using UnityEngine.UI;

public class LockedDrawerPuzzle : Puzzle
{
    private GameObject digits;
    private GameObject digitsSecondary;
    private GameObject remote;

    void Awake()
    {
        answer = new char[] { '2', '3', '5', '7' };
        guess = new char[] { '0', '0', '0', '0' };

        digits = transform.Find("Digits").gameObject;
        digitsSecondary = transform.Find("DigitsSecondary").gameObject;
        remote = transform.Find("Remote").gameObject;

        digitsSecondary.SetActive(false);
        remote.SetActive(false);
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["LockedDrawerPuzzleSecondary"];
        
        digits.SetActive(false);
        
        digitsSecondary.SetActive(true);
        remote.SetActive(true);

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionLockedDrawerPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedLockedDrawerPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}
