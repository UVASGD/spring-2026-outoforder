using UnityEngine.UI;

public class LockedDrawerPuzzle : Puzzle
{
    void Awake()
    {
        answer = new char[] { '2', '3', '5', '7' };
        guess = new char[] { '0', '0', '0', '0' };
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["UnlockedDrawerPuzzle"];
        
        GameProgression.GameProgressionInstance.SetFlag("firstLockedDrawerPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedLockedDrawerPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}
