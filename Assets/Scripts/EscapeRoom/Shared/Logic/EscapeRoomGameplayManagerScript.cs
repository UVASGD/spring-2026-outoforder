using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EscapeRoomGameplayManagerScript : MonoBehaviour
{
    public List<string> locationNames;
    public List<GameObject> locations;
    public List<ItemData> items;
    public HashSet<string> collectedItems = new();
    public Dictionary<string, string> itemDescriptions = new();

    void Awake()
    {
        GameData.escapeRoomGameplayManagerScript = this;
        
        locationNames = JsonConvert.DeserializeObject<List<string>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/{GameData.escapeRoomNumber}_locations").text);
        locations.AddRange(Enumerable.Range(0, 5).Select(i => GameObject.Find($"Location{i}")));

        items = JsonConvert.DeserializeObject<List<ItemData>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/{GameData.escapeRoomNumber}_items").text);
        foreach (ItemData item in items)
        {
            itemDescriptions.Add(item.name, item.description);
        }
    }
}
