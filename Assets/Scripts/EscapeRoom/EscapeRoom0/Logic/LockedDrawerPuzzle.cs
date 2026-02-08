using UnityEngine;
using UnityEngine.UI;

public class LockedDrawerPuzzle : Puzzle
{
    private GameObject remote;

    void Awake()
    {
        answer = new char[] { '2', '3', '5', '7' };
        guess = new char[] { '0', '0', '0', '0' };

        remote = transform.Find("Remote").gameObject;
        remote.SetActive(false);
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["LockedDrawerPuzzleSecondary"];
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        remote.SetActive(true);

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionLockedDrawerPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedLockedDrawerPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}
