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
        { "GearA", 51 },
        { "GearB", 12 },
        { "GearC", 34 },
        { "GearD", 50 },
        { "GearE", 43 },
        { "GearF", 8 },
        { "GearG", 41 },
        { "GearH", 200 },
        { "GearI", 56 },
        { "GearJ", 38 }
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

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            sum += gearWeights[transform.GetChild(0).name];
        }

        print($"the sum is {sum}");

        guess[0] = (sum == 250) ? '1' : '0';
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