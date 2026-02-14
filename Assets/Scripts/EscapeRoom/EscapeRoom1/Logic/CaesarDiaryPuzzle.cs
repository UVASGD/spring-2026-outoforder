using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaesarDiaryPuzzle : Puzzle
{
    public TMP_Text letter0;
    public TMP_Text letter1;
    public TMP_Text letter2;
    public TMP_Text letter3;
    public TMP_Text letter4;
    public TMP_Text letter5;
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

    public void ResetPuzzle()
    {
        letter0.text = "C";
        letter1.text = "A";
        letter2.text = "E";
        letter3.text = "S";
        letter4.text = "A";
        letter5.text = "R";
        guess = new char[] { 'C', 'A', 'E', 'S', 'A', 'R' };
    }
}
