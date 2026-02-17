using UnityEngine;
using UnityEngine.UI;

public class CounterweightSheavePuzzle : Puzzle
{
    void Awake()
    {
        // answer = new char[] { 'R', 'E', 'A', 'D' };
        // guess = new char[] { 'A', 'B', 'C', 'D' };
    }

    protected override void SolvedPuzzleSpecific()
    {
        // gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["UnlockedLockedLockerPuzzle"];

        // caesarDiary.SetActive(true);

        // // TODO: THIS COULD PROBABLY BE A METHOD IN PUZZLE THIS CODE IS REPEATED A LOT
        // GameProgression.GameProgressionInstance.SetFlag("firstInteractionLockedLockerPuzzle", true);
        // GameProgression.GameProgressionInstance.SetFlag("solvedLockedLockerPuzzle", true);
        // gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}
