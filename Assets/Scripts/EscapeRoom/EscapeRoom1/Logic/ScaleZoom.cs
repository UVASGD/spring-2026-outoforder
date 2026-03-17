using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ScaleZoom : MonoBehaviour
{
    private GameObject emptyGear;
    private ItemController itemController;
    private Image image;
    private TextMeshProUGUI scaleText;
    private Dictionary<string, string> gearWeights = new Dictionary<string, string>
    {
        { "GearA", "051" },
        { "GearB", "012" },
        { "GearC", "034" },
        { "GearD", "050" },
        { "GearE", "043" },
        { "GearF", "008" },
        { "GearG", "041" },
        { "GearH", "200" },
        { "GearI", "056" },
        { "GearJ", "038" }
    };
    private Color white = new Color(1, 1, 1, 1);
    private Color transparentWhite = new Color(1, 1, 1, 0);

    void Awake()
    {
        scaleText = GetComponentInChildren<TextMeshProUGUI>();

        emptyGear = transform.Find("EmptyGear").gameObject;
        itemController = emptyGear.GetComponent<ItemController>();
        image = emptyGear.GetComponent<Image>();
    }

    public void AttemptPlaceGear()
    {
        if (!string.IsNullOrEmpty(GameData.escapeRoomGameplayManager.selectedItem) && emptyGear.name.Equals("EmptyGear"))
        {
            if (GameData.escapeRoomGameplayManager.selectedItem.Contains("Gear"))
            {
                print("display weight");
                scaleText.text = $"{gearWeights[GameData.escapeRoomGameplayManager.selectedItem.Replace(" ", "")]}";
                if (scaleText.text.Equals("200"))
                {
                    GameProgression.GameProgressionInstance.SetFlag("firstInteractionScaleZoom", true);
                    GameProgression.GameProgressionInstance.SetFlag("heavyInteractionScaleZoom", true);
                }

                emptyGear.name = GameData.escapeRoomGameplayManager.selectedItem.Replace(" ", "");
                image.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["Item"];
                image.color = white;
                GameData.escapeRoomGameplayManager.UseItem(GameData.escapeRoomGameplayManager.selectedItem, emptyGear.name);
                print("place gear");

                GameData.escapeRoomGameplayManager.locations.ExamineItem();
            }
        }
        else if (itemController.itemData.collectible)
        {
            scaleText.text = "000";
            emptyGear.name = "EmptyGear";
            // TODO: make an image that is just white or something
            image.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["Textbox"];
            image.color = transparentWhite;
            GameData.escapeRoomGameplayManager.locations.CollectItem(itemController.itemData, emptyGear);
            print("remove gear");
        }

        itemController.UpdateItemData();
    }

    public void DisplayPlacedGear()
    {
        emptyGear.SetActive(true);
    }
}
