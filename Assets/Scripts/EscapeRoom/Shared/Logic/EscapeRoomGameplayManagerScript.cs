using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EscapeRoomGameplayManagerScript : MonoBehaviour
{
    public List<string> locationNames;
    public List<GameObject> locations;
    
    public string selectedItem;
    public Dictionary<string, ItemData> items = new();
    public Dictionary<string, GameObject> collectedItemsScrollView = new();
    public Dictionary<string, string> itemDescriptions = new();
    
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
    }

    public bool UseItem(string directItem, string indirectItem)
    {
        print($"selected item is {selectedItem}! and interactingWith is {interactingWith}!");
        // TODO MAYBE SOME SORT OF USED ITEM SOUND EFFECT
        if (selectedItem.Equals(directItem) && interactingWith.Equals(indirectItem))
        {
            Destroy(collectedItemsScrollView[directItem]);
            collectedItemsScrollView.Remove(directItem);
            GameProgression.GameProgressionInstance.SetFlag($"used{directItem}", true);
            return true;
        }
        return false;
    }
}
