using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EscapeRoomGameplay : MonoBehaviour
{
    public Locations locations;
    public MenuBar menuBar;

    public List<string> locationNames;
    public List<GameObject> locationGameObjects;

    public Dictionary<string, GameObject> puzzles = new();
    
    public string selectedItem;
    public Dictionary<string, ItemData> items = new();
    public Dictionary<string, GameObject> collectedItemsScrollView = new();
    // TODO: delete?
    public Dictionary<string, string> itemDescriptions = new();
    public Dictionary<string, string> usages = new();
    
    public string interactingWith;
    public bool enteredItem;

    public HashSet<string> eventTracker = new();

    void Awake()
    {
        GameData.escapeRoomGameplayManager = this;
        
        locationNames = JsonConvert.DeserializeObject<List<string>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/Data/locations").text);
        locationGameObjects.AddRange(Enumerable.Range(0, 5).Select(i => GameObject.Find($"Location{i}")));

        items = JsonConvert.DeserializeObject<Dictionary<string, ItemData>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/Data/items").text);
        foreach (ItemData item in items.Values)
        {
            itemDescriptions.Add(item.name, item.description);
        }
        
        usages = JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>($"Story/EscapeRoom/EscapeRoom{GameData.escapeRoomNumber}/Data/usages").text);
    }

    void Update()
    {
        CheckForEvents(GameData.escapeRoomNumber);
    }

    public bool UseItem(string directItem, string indirectItem)
    {
        print($"attempt to use item {directItem} on {indirectItem}");
        // TODO MAYBE SOME SORT OF USED ITEM SOUND EFFECT
        if (usages.TryGetValue(directItem.Replace(" ", ""), out var value) && value == indirectItem)
        {
            selectedItem = "";
            Destroy(collectedItemsScrollView[directItem]);
            collectedItemsScrollView.Remove(directItem);
            GameProgression.GameProgressionInstance.SetFlag($"used{directItem.Replace(" ", "")}", true);
            return true;
        }
        return false;
    }

    private void CheckForEvents(int escapeRoomNumber)
    {
        switch (escapeRoomNumber)
        {
            case 0:
                // music starts again after finishing interaction with ASTA for the first time
                if (!eventTracker.Contains("firstInteractionRobot") && !GameData.currentlyTalking && GameProgression.GameProgressionInstance.GetFlag("firstInteractionRobot"))
                {
                    eventTracker.Add("firstInteractionRobot");
                    StartCoroutine(GameProgression.GameProgressionInstance.PlayBGM(3));
                }

                if (!eventTracker.Contains("filledDullYellowCore") && GameProgression.GameProgressionInstance.GetFlag("filledDullYellowCore"))
                {
                    eventTracker.Add("filledDullYellowCore");
                    StartCoroutine(FindFirstObjectByType<AntifreezeDispenserPuzzle>().ActivateAntifreezeDispenser());
                }

                break;
            default:
                break;
        }
    }
}
