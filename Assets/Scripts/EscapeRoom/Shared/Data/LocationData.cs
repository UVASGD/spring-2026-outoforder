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
        UpdateLocationText();
    }

    public void UpdateLocationText()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = GameData.escapeRoomGameplayManagerScript.locationNames[locationIndex];
    }
}