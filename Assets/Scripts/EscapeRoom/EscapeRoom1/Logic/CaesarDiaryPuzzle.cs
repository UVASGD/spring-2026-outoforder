using UnityEngine;
using UnityEngine.UI;

public class CaesarDiaryPuzzle : Puzzle
{
    void Awake()
    {
        answer = new char[] { 'K', 'I', 'M', 'A', 'I', 'Z' };
        guess = new char[] { 'C', 'A', 'E', 'S', 'A', 'R' };
    }

    protected override void SolvedPuzzleSpecific()
    {
        //GameData.escapeRoomGameplayManagerScript.locations.ForEach(location => location.GetComponent<Image>().color = Color.white);

        //gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["UnlockedLightSwitchPuzzle"];

        //GameProgression.GameProgressionInstance.SetFlag("firstLightSwitchPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedCaesarDiaryPuzzle", true);
    }
}
