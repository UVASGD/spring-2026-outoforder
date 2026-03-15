using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterweightSheavePuzzle : Puzzle
{
    private GameObject submit;
    private bool compartmentUnlocked;
    private Dictionary<string, int> gearWeights = new Dictionary<string, int>
    {
        { "Gear A", 51 },
        { "Gear B", 12 },
        { "Gear C", 34 },
        { "Gear D", 50 },
        { "Gear E", 43 },
        { "Gear F", 8 },
        { "Gear G", 41 },
        { "Gear H", 200 },
        { "Gear I", 56 },
        { "Gear J", 38 }
    };

    void Awake()
    {
        answer = new char[] { '1' };
        guess = new char[] { '0' };

        submit = transform.Find("Submit").gameObject;
    }

    void Update()
    {
        if (!compartmentUnlocked && GameProgression.GameProgressionInstance.GetFlag("usedKey"))
        {
            compartmentUnlocked = true;
            submit.GetComponent<Button>().enabled = true;
            submit.GetComponent<Image>().enabled = true;

            GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CounterweightSheavePuzzleSecondary"];
        }
    }

    protected override void ConvertGuess()
    {
        int sum = 0;

        foreach (GameObject gear in gameObject.transform)
        {
            sum += gearWeights[gear.name];
        }

        answer[0] = (sum == 250) ? '1' : '0';
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
