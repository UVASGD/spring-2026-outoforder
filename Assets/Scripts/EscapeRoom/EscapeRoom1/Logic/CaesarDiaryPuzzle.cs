using UnityEngine;
using UnityEngine.UI;

public class CaesarDiaryPuzzle : Puzzle
{
    public GameObject submit;
    private bool leverRepaired;

    void Awake()
    {
        answer = new char[] { 'K', 'I', 'M', 'A', 'I', 'Z' };
        guess = new char[] { 'A', 'A', 'A', 'A', 'A', 'A' };

        submit = transform.Find("Submit").gameObject;
    }

    void Update()
    {
        if (!leverRepaired && GameProgression.GameProgressionInstance.GetFlag("firstInteractionLightSwitchPuzzle"))
        {
            leverRepaired = true;
            submit.GetComponent<Button>().enabled = true;
            submit.GetComponent<Image>().enabled = true;
        }
    }

    protected override void SolvedPuzzleSpecific()
    {
        GameData.escapeRoomGameplayManagerScript.locations.ForEach(location => location.GetComponent<Image>().color = Color.white);

        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["UnlockedLightSwitchPuzzle"];

        GameProgression.GameProgressionInstance.SetFlag("firstLightSwitchPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedLightSwitchPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
        GameProgression.GameProgressionInstance.SetFlag("lastInteractionLightSwitchPuzzle", true);
    }
}
