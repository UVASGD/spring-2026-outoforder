using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// TODO: split this into to MovePopUpScript
public class LocationsScript : MonoBehaviour
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

    private Dictionary<string, GameObject> puzzles = new();

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
        currentLocationTMP.text = GameData.escapeRoomGameplayManagerScript.locations[0];

        content = popUpAreas.transform.Find("ItemPopUp").transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

        puzzles = FindObjectsByType<Puzzle>(FindObjectsSortMode.None).OfType<MonoBehaviour>().ToDictionary(
            puzzle => puzzle.gameObject.name,
            puzzle => puzzle.gameObject
        );

        foreach (GameObject puzzle in puzzles.Values)
        {
            puzzle.SetActive(false);
        }
    }

    public void ExamineItem()
    {
        ManualInteraction manualInteraction = EventSystem.current.currentSelectedGameObject.GetComponent<ManualInteraction>();
        InteractItem(manualInteraction);

        ItemController itemController = EventSystem.current.currentSelectedGameObject.GetComponent<ItemController>();
        if (itemController != null)
        {
            ItemData itemData = itemController.itemData;
            int itemIndex = itemController.itemIndex;
            print($"looking at {itemData.name}");
            CollectItem(itemData, itemIndex);
            ZoomItem(itemData);
        }
    }

    public void ZoomItem(ItemData itemData)
    {
        if (itemData.detailed) StartCoroutine(WaitToZoomItem(itemData));
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

    // HELPERS
    private void InteractItem(ManualInteraction manualInteraction)
    {
        manualInteraction.ItemInteraction();
    }

    private void CollectItem(ItemData itemData, int itemIndex)
    {
        if (itemData.collectible && !GameData.escapeRoomGameplayManagerScript.collectedItems.Contains(itemData.name))
        {
            print($"collecting {itemData.name}");
            GameObject item = Instantiate(Resources.Load<GameObject>("Prefabs/ScrollViewItem"), content.transform);
            item.GetComponent<ItemController>().itemIndex = itemIndex;
            item.GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
            GameData.escapeRoomGameplayManagerScript.collectedItems.Add(itemData.name);
        }
    }

    private void ShowLocation(GameObject location, int newLocationIndex)
    {
        currentLocationIndex = newLocationIndex;
        // TODO: make this into an animation with coroutine
        currentLocation?.SetActive(false);
        location.SetActive(true);
        currentLocation = location;
        currentLocationTMP.text = GameData.escapeRoomGameplayManagerScript.locations[currentLocationIndex];
    }

    private IEnumerator WaitToZoomItem(ItemData itemData)
    {
        while (GameData.currentlyTalking)
        {
            yield return null;
        }

        puzzles[itemData.name.Replace(" ", "") + "Puzzle"].gameObject.SetActive(true);
    }
}