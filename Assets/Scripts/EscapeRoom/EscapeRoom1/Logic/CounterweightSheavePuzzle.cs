using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CounterweightSheavePuzzle : Puzzle
{
    private GameObject submit;
    private GameObject lastActiveSlot;
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

        print($"the sum is {sum}");

        answer[0] = (sum == 250) ? '1' : '0';
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CounterweightSheaveSecondary"];

        // TODO: THIS COULD PROBABLY BE A METHOD IN PUZZLE THIS CODE IS REPEATED A LOT
        GameProgression.GameProgressionInstance.SetFlag("secondInteractionCounterweightSheavePuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedCounterweightSheavePuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }

    public void ModifyGear()
    {
        lastActiveSlot = EventSystem.current.currentSelectedGameObject;
        ItemController itemController = lastActiveSlot.GetComponent<ItemController>();
        Image image = lastActiveSlot.GetComponent<Image>();
        
        if (lastActiveSlot.GetComponent<Image>().sprite == null)
        {
            if (GameData.escapeRoomGameplayManager.selectedItem.Contains("Gear"))
            {
                lastActiveSlot.name = GameData.escapeRoomGameplayManager.selectedItem.Replace(" ", "");
                image.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["Item"];
                GameData.escapeRoomGameplayManager.UseItem(GameData.escapeRoomGameplayManager.selectedItem, lastActiveSlot.name);
                print("place gear");
            }
        }
        else
        {
            lastActiveSlot.name = "EmptySlot";
            image.sprite = null;
            GameData.escapeRoomGameplayManager.locations.CollectItem(itemController.itemData, lastActiveSlot);
            print("remove gear");
        }

        itemController.UpdateItemData();
    }

    public void ActivateGear()
    {
        lastActiveSlot.SetActive(true);
    }
}