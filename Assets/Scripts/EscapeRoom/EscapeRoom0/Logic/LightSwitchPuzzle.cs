using UnityEngine;
using UnityEngine.UI;

public class LightSwitchPuzzle : Puzzle
{
    public GameObject submit;
    private bool leverRepaired;

    void Awake()
    {
        answer = new char[] { '9', '0', '6', '0', '1', '2', '0' };
        guess = new char[] { '0', '0', '0', '0', '0', '0', '0' };

        submit = transform.Find("Submit").gameObject;
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
        GameData.escapeRoomGameplayManagerScript.locations.ForEach(location => location.GetComponent<Image>().color = Color.white);

        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["UnlockedLightSwitchPuzzle"];

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionLightSwitchPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedLightSwitchPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
        GameProgression.GameProgressionInstance.SetFlag("lastInteractionLightSwitchPuzzle", true);
    }
}
