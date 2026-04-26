using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverManagerTMP : MonoBehaviour
{
    [Header("UI References")]
    private Vector2 offset = new Vector2(275, -20);
    private GameObject hover;
    private RectTransform hoverImage;
    private TextMeshProUGUI hoverText;
    private HashSet<GameObject> manualObjects = new();
    private GraphicRaycaster raycaster;
    private PointerEventData pointerData;
    private EventSystem eventSystem;

    void Awake()
    {
        hover = GameObject.Find("Canvas/Hover");
        hoverImage = hover.transform.Find("HoverImage")?.GetComponent<RectTransform>();
        hoverText = hover.transform.Find("HoverText").GetComponent<TextMeshProUGUI>();
        
        raycaster = hoverText.GetComponentInParent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        if (eventSystem == null) eventSystem = FindFirstObjectByType<EventSystem>();

        ManualInteraction[] manualInteractions = FindObjectsByType<ManualInteraction>(FindObjectsSortMode.None);
        foreach (ManualInteraction mi in manualInteractions)
        {
            manualObjects.Add(mi.gameObject);
        }

        hover.gameObject.SetActive(false);
    }

    void Update()
    {
        pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        bool foundManual = false;

        foreach (RaycastResult result in results)
        {
            if (manualObjects.Contains(result.gameObject))
            {
                hover.gameObject.SetActive(true);
                hoverText.text = $"(?) {result.gameObject.name}";
                hover.transform.position = Input.mousePosition + (Vector3)offset;
                foundManual = true;
                break;
            }
        }

        if (!foundManual || GameData.currentlyTalking || !GameData.escapeRoomGameplayManager.menuBar.currentMenuName.Equals("none"))
        {
            hover.gameObject.SetActive(false);
        }
    }
}