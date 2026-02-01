using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// TODO: rename to LocationController
public class LocationData : MonoBehaviour
{
    public int locationIndex;

    void Start()
    {
        GameData.escapeRoomGameplayManagerScript.locationNames = JsonConvert.DeserializeObject<List<string>>(Resources.Load<TextAsset>("Story/EscapeRoom/EscapeRoom0/0_locations").text);
        UpdateLocationText();
    }

    public void UpdateLocationText()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = GameData.escapeRoomGameplayManagerScript.locationNames[locationIndex];
    }
}