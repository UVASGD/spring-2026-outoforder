using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// TODO: split this into to MovePopUpScript
public class Locations : MonoBehaviour
{
    private ManualInteraction action;
    private List<GameObject> locations;
    private int currentLocationIndex;
    private GameObject currentLocation;
    private TextMeshProUGUI currentLocationTMP;
    private GameObject content;
    private string currentPuzzle;
    private Button backButton;
    private int itemsEntered;

    // DEBUG ONLY -- HOW TO CHEAT ITEMS INTO YOUR INVENTORY
    [SerializeField] List<string> debugItemNames = new();
    List<ItemData> debugItemList = new();

    void Awake()
    {
        GameData.escapeRoomGameplayManager.locations = this;

        action = GameObject.Find("Action")?.GetComponent<ManualInteraction>();

        // DEBUG ONLY -- HOW TO CHEAT ITEMS INTO YOUR INVENTORY
        debugItemNames.ForEach(name => debugItemList.Add(GameData.escapeRoomGameplayManager.items[name]));
    }

    void Start()
    {
        locations = Enumerable.Range(0, 5).Select(i => transform.Find($"Location{i}").gameObject).ToList();

        GameObject popUpAreas = transform.parent.transform.Find("MenuBar").transform.Find("PopUpAreas").gameObject;

        currentLocation = locations[0];
        currentLocationTMP = popUpAreas.transform.Find("MovePopUp").transform.Find("CurrentLocationText").GetComponent<TextMeshProUGUI>();
        currentLocationTMP.text = GameData.escapeRoomGameplayManager.locationNames[0];

        content = popUpAreas.transform.Find("ItemPopUp").transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

        GameData.escapeRoomGameplayManager.puzzles = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(item => 
                item.gameObject.name.Contains("Zoom", System.StringComparison.OrdinalIgnoreCase) || 
                item is Puzzle
            )
            .GroupBy(item => item.gameObject.name)
            .ToDictionary(
                group => group.Key, 
                group => group.First().gameObject
            );

        foreach (GameObject puzzle in GameData.escapeRoomGameplayManager.puzzles.Values)
        {
            puzzle.SetActive(false);
        }

        backButton = transform.parent.transform.Find("BackButton").GetComponent<Button>();
        backButton.gameObject.SetActive(false);

        locations.Skip(1).ToList().ForEach(loc => loc.SetActive(false));
        
        // DEBUG ONLY -- HOW TO CHEAT ITEMS INTO YOUR INVENTORY
        foreach (ItemData debugItem in debugItemList)
        {
            GameObject scrollViewItem = Instantiate(Resources.Load<GameObject>("Prefabs/ScrollViewItem"), content.transform);
            scrollViewItem.name = debugItem.name.Replace(" ", "");
            scrollViewItem.GetComponentInChildren<TextMeshProUGUI>().text = debugItem.name;
            GameData.escapeRoomGameplayManager.collectedItemsScrollView[debugItem.name] = scrollViewItem;
        }
    }

    public void ExamineItem()
    {
        GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        ManualInteraction manualInteraction = currentSelectedGameObject.GetComponent<ManualInteraction>();
        InteractItem(manualInteraction);

        ItemController itemController = currentSelectedGameObject.GetComponent<ItemController>();
        if (itemController != null)
        {
            ItemData itemData = itemController.itemData;
            print($"looking at {itemData.name}");
            CollectItem(itemData, currentSelectedGameObject);
            // TODO: maybe more specific conditions of not entering an item in later rooms
            if ((GameData.escapeRoomNumber == 0 && (itemData.name.Equals("Light Switch") || GameProgression.GameProgressionInstance.GetFlag("solvedLightSwitchPuzzle")))
                || GameData.escapeRoomNumber > 0) 
            {
                EnterItem(itemData);
            }
        }
    }

    // PUBLIC HELPERS
    public void CollectItem(ItemData itemData, GameObject item)
    {
        if (!GameProgression.GameProgressionInstance.GetFlag($"used{itemData.name}") && itemData.collectible && !GameData.escapeRoomGameplayManager.collectedItemsScrollView.ContainsKey(itemData.name))
        {
            print($"collecting {itemData.name}");
            
            item.SetActive(false);

            GameObject scrollViewItem = Instantiate(Resources.Load<GameObject>("Prefabs/ScrollViewItem"), content.transform);
            scrollViewItem.name = itemData.name.Replace(" ", "");
            scrollViewItem.GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
            GameData.escapeRoomGameplayManager.collectedItemsScrollView[itemData.name] = scrollViewItem;
        }
    }

    public void EnterItem(ItemData itemData)
    {
        if (itemData.detailed)
        {
            StartCoroutine(WaitToEnterItem(itemData));
        }
    }
    
    public void ExitItem()
    {
        GameProgression.GameProgressionInstance.PlaySFX(2);

        itemsEntered--;
        GameData.escapeRoomGameplayManager.puzzles[currentPuzzle].gameObject.SetActive(false);
        if (itemsEntered == 0) 
        {
            GameData.escapeRoomGameplayManager.enteredItem = false;
            backButton.gameObject.SetActive(false);
            currentPuzzle = "";
        }
        else
        {
            currentPuzzle = GameData.escapeRoomGameplayManager.puzzles[currentPuzzle].transform.parent.parent.name;
        }
    }

    public void ChangeLocation(string overrideLocation = null)
    {
        GameObject currentSelectedLocation = string.IsNullOrEmpty(overrideLocation)
            ? EventSystem.current.currentSelectedGameObject
            : transform.parent.GetComponentsInChildren<TextMeshProUGUI>(true).FirstOrDefault(t => t.text.Equals(overrideLocation))?.transform.parent.gameObject;

        LocationData currentLocationData = currentSelectedLocation.GetComponent<LocationData>();

        int newLocationIndex = currentLocationData.locationIndex;

        switch (newLocationIndex)
        {
            case 0:
                ShowLocation(locations[0], newLocationIndex, currentLocationData);
                break;
            case 1:
                ShowLocation(locations[1], newLocationIndex, currentLocationData);
                break;
            case 2:
                ShowLocation(locations[2], newLocationIndex, currentLocationData);
                break;
            case 3:
                if ((GameData.escapeRoomNumber == 0 && GameProgression.GameProgressionInstance.GetFlag("firstInteractionRobot"))
                    || GameData.escapeRoomNumber > 0)
                {
                    ShowLocation(locations[3], newLocationIndex, currentLocationData);   
                }
                else
                {
                    action.ItemInteraction();
                    return;
                }
                break;
            case 4:
                ShowLocation(locations[4], newLocationIndex, currentLocationData);
                break;
        }
        
        currentLocationData.UpdateLocationText();
    }

    // PRIVATE HELPERS
    private void InteractItem(ManualInteraction manualInteraction)
    {
        manualInteraction.ItemInteraction();
    }

    private void ShowLocation(GameObject location, int newLocationIndex, LocationData currentLocationData)
    {
        GameProgression.GameProgressionInstance.PlaySFX(3);

        currentLocationData.locationIndex = currentLocationIndex;
        currentLocationIndex = newLocationIndex;
        // TODO: make this into an animation with coroutine
        currentLocation?.SetActive(false);
        location.SetActive(true);
        currentLocation = location;
        currentLocationTMP.text = GameData.escapeRoomGameplayManager.locationNames[currentLocationIndex];
    }

    private IEnumerator WaitToEnterItem(ItemData itemData)
    {
        while (GameData.currentlyTalking)
        {
            yield return null;
        }

        itemsEntered++;
        GameData.escapeRoomGameplayManager.enteredItem = true;
        currentPuzzle = itemData.name.Replace(" ", "") + (itemData.puzzle ? "Puzzle" : "Zoom");
        GameData.escapeRoomGameplayManager.puzzles[currentPuzzle].gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }
}