using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CounterweightSheavePuzzle : Puzzle
{
    private GameObject lastActiveSlot;
    private ItemController itemController;
    private Image image;
    private GameObject submit;
    private bool compartmentUnlocked;
    private Dictionary<string, int> gearWeights = new Dictionary<string, int>
    {
        { "GearA", 51 },
        { "GearB", 22 },
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
        lastActiveSlot = transform.Find("EmptySlot").gameObject;
        itemController = lastActiveSlot.GetComponent<ItemController>();
        image = lastActiveSlot.GetComponent<Image>();

        lastActiveSlot.SetActive(false);
    }

    void Update()
    {
        if (!compartmentUnlocked && GameProgression.GameProgressionInstance.GetFlag("usedKey"))
        {
            GameProgression.GameProgressionInstance.SetFlag("firstInteractionCounterweightSheavePuzzle", true);

            compartmentUnlocked = true;
            submit.GetComponent<Button>().enabled = true;
            submit.GetComponent<Image>().enabled = true;

            lastActiveSlot.SetActive(true);

            GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CounterweightSheavePuzzleSecondary"];
        }
    }

    protected override void ConvertGuess()
    {
        int sum = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            string childName = transform.GetChild(i).name;
            if (childName.Contains("Gear")) sum += gearWeights[childName];
        }

        print($"the sum is {sum}");

        guess[0] = (sum == 250) ? '1' : '0';
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CounterweightSheavePuzzleSecondary"];

        // TODO: THIS COULD PROBABLY BE A METHOD IN PUZZLE THIS CODE IS REPEATED A LOT
        GameProgression.GameProgressionInstance.SetFlag("solvedCounterweightSheavePuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }

    public void ModifyGear()
    {
        lastActiveSlot = EventSystem.current.currentSelectedGameObject;
        itemController = lastActiveSlot.GetComponent<ItemController>();
        image = lastActiveSlot.GetComponent<Image>();

        if (!string.IsNullOrEmpty(GameData.escapeRoomGameplayManager.selectedItem))
        {   
            if (image.sprite == null)
            {
                if (GameData.escapeRoomGameplayManager.selectedItem.Contains("Gear"))
                {
                    lastActiveSlot.name = GameData.escapeRoomGameplayManager.selectedItem.Replace(" ", "");
                    image.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[$"{lastActiveSlot.name}"];
                    GameData.escapeRoomGameplayManager.UseItem(GameData.escapeRoomGameplayManager.selectedItem, lastActiveSlot.name);

                    // TODO: but the last one shouldn't be visible unless the count were to drop down to 9 gears on display
                    if (transform.childCount <= 11)
                    {
                        GameObject newActiveSlot = Instantiate(Resources.Load<GameObject>("Prefabs/EmptySlot"), transform);
                        newActiveSlot.name = "EmptySlot";
                        newActiveSlot.GetComponent<ItemController>().enabled = true;
                        newActiveSlot.transform.position = new Vector3(lastActiveSlot.transform.position.x, lastActiveSlot.transform.position.y + 60, lastActiveSlot.transform.position.z);
                        newActiveSlot.transform.SetAsLastSibling();
                        newActiveSlot.GetComponent<Button>().onClick.AddListener(ModifyGear);
                    }

                    print("place gear");
                }
            }
        }
        else
        {
            if (!lastActiveSlot.name.Equals("EmptySlot"))
            {
                image.sprite = null;
                GameData.escapeRoomGameplayManager.locations.CollectItem(itemController.itemData, lastActiveSlot);
                
                for (int i = lastActiveSlot.transform.GetSiblingIndex(); i < transform.childCount; i++)
                {
                    Vector3 childPosition = transform.GetChild(i).transform.position;
                    transform.GetChild(i).transform.position = new Vector3(childPosition.x, childPosition.y - 60, childPosition.z);
                }

                Destroy(lastActiveSlot);

                print("remove gear");
            }  
        }

        itemController.UpdateItemData();
        lastActiveSlot.SetActive(true);
    }
}