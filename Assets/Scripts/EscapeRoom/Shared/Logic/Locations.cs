using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// TODO: split this into to MovePopUpScript
public class Locations : MonoBehaviour
{
    private GameObject location0;
    private GameObject location1;
    private GameObject location2;
    private GameObject location3;
    private GameObject location4;
    private int currentLocationIndex;
    private GameObject currentLocation;
    private TextMeshProUGUI currentLocationTMP;
    private GameObject content;
    private string currentPuzzle;
    private Button backButton;
    private int itemsEntered;

    // DEBUG ONLY -- HOW TO CHEAT ITEMS INTO YOUR INVENTORY
    ItemData debugItemData;

    void Awake()
    {
        GameData.escapeRoomGameplayManager.locations = this;
        
        // DEBUG ONLY -- HOW TO CHEAT ITEMS INTO YOUR INVENTORY
        debugItemData = GameData.escapeRoomGameplayManager.items["Key"];
    }

    void Start()
    {
        location0 = transform.Find("Location0").gameObject;
        location1 = transform.Find("Location1").gameObject;
        location2 = transform.Find("Location2").gameObject;
        location3 = transform.Find("Location3").gameObject;
        location4 = transform.Find("Location4").gameObject;

        GameObject popUpAreas = transform.parent.transform.Find("MenuBar").transform.Find("PopUpAreas").gameObject;

        currentLocation = location0;
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

        location1.SetActive(false);
        location2.SetActive(false);
        location3.SetActive(false);
        location4.SetActive(false);
        
        // DEBUG ONLY -- HOW TO CHEAT ITEMS INTO YOUR INVENTORY
        GameObject scrollViewItem = Instantiate(Resources.Load<GameObject>("Prefabs/ScrollViewItem"), content.transform);
        scrollViewItem.name = debugItemData.name.Replace(" ", "");
        scrollViewItem.GetComponentInChildren<TextMeshProUGUI>().text = debugItemData.name;
        GameData.escapeRoomGameplayManager.collectedItemsScrollView[debugItemData.name] = scrollViewItem;
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

    public void ChangeLocation()
    {
        GameObject currentSelectedLocation = EventSystem.current.currentSelectedGameObject;
        
        LocationData currentLocationData = currentSelectedLocation.GetComponent<LocationData>();
        int newLocationIndex = currentLocationData.locationIndex;
        currentLocationData.locationIndex = currentLocationIndex;
        
        switch (newLocationIndex)
        {
            case 0:
                ShowLocation(location0, newLocationIndex);
                break;
            case 1:
                ShowLocation(location1, newLocationIndex);
                break;
            case 2:
                ShowLocation(location2, newLocationIndex);
                break;
            case 3:
                ShowLocation(location3, newLocationIndex);
                break;
            case 4:
                ShowLocation(location4, newLocationIndex);
                break;
        }
    }

    // PRIVATE HELPERS
    private void InteractItem(ManualInteraction manualInteraction)
    {
        manualInteraction.ItemInteraction();
    }

    private void ShowLocation(GameObject location, int newLocationIndex)
    {
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