using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EscapeRoomGameplay : MonoBehaviour
{
    public List<string> locationNames;
    public List<GameObject> locations;
    
    public string selectedItem;
    public Dictionary<string, ItemData> items = new();
    public Dictionary<string, GameObject> collectedItemsScrollView = new();
    public Dictionary<string, string> itemDescriptions = new();
    public Dictionary<string, string> usages = new();
    
    public string interactingWith;
    public bool enteredItem;

    void Awake()
    {
        GameData.escapeRoomGameplayManagerScript = this;
        
        locationNames = JsonConvert.DeserializeObject<List<string>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/Data/locations").text);
        locations.AddRange(Enumerable.Range(0, 5).Select(i => GameObject.Find($"Location{i}")));

        items = JsonConvert.DeserializeObject<Dictionary<string, ItemData>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/Data/items").text);
        foreach (ItemData item in items.Values)
        {
            itemDescriptions.Add(item.name, item.description);
        }
        
        usages = JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/Data/usages").text);
    }

    public bool UseItem(string directItem, string indirectItem)
    {
        print($"using {directItem} on {indirectItem}");
        // TODO MAYBE SOME SORT OF USED ITEM SOUND EFFECT
        if (usages.TryGetValue(directItem.Replace(" ", ""), out var value) && value == indirectItem)
        {
            print("yes used");
            selectedItem = "";
            Destroy(collectedItemsScrollView[directItem]);
            collectedItemsScrollView.Remove(directItem);
            GameProgression.GameProgressionInstance.SetFlag($"used{directItem.Replace(" ", "")}", true);
            return true;
        }
        return false;
    }
}
