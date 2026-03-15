using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ScaleZoom : MonoBehaviour
{
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

    void Awake()
    {
        scaleText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AttemptPlaceGear()
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
        }
    }
}
