using UnityEngine;
using UnityEngine.UI;

public class LockedLockerPuzzle : Puzzle
{
    private GameObject caesarDiary;

    void Awake()
    {
        answer = new char[] { 'R', 'E', 'A', 'D' };
        guess = new char[] { 'A', 'B', 'C', 'D' };

        caesarDiary = transform.Find("Interactables").Find("CaesarDiary").gameObject;
        caesarDiary.SetActive(false);
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["UnlockedLockedLockerPuzzle"];

        caesarDiary.SetActive(true);

        GameProgression.GameProgressionInstance.SetFlag("solvedLockedLockerPuzzle", true);
    }
}
