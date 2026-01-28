using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class EscapeRoomGameplayManagerScript : MonoBehaviour
{
    public List<string> locations;
    public List<ItemData> items;
    public HashSet<string> collectedItems = new();
    public Dictionary<string, string> itemDescriptions = new();

    void Awake()
    {
        GameData.escapeRoomGameplayManagerScript = this;
        locations = JsonConvert.DeserializeObject<List<string>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/{GameData.escapeRoomNumber}_locations").text);
        items = JsonConvert.DeserializeObject<List<ItemData>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/{GameData.escapeRoomNumber}_items").text);
    
        foreach (ItemData item in items)
        {
            itemDescriptions.Add(item.name, item.description);
        }
    }
}
