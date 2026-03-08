using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPopUp : MonoBehaviour
{
    private string displayedItem;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private Button selectButton;
    private GameObject content;

    void Awake()
    {
        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        descriptionText = transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        selectButton = transform.Find("SelectButton").GetComponent<Button>();
        content = transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").gameObject;

        selectButton.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (GameData.escapeRoomGameplayManagerScript.collectedItemsScrollView.Count > 0)
        {
            ShowItem(content.transform.GetChild(0).GetComponent<ItemController>().itemData);
            selectButton.gameObject.SetActive(true);
        }
        else
        {
            displayedItem = nameText.text = descriptionText.text = "";
            selectButton.gameObject.SetActive(false);
        }
    }

    public void ShowItem(ItemData itemData = null)
    {
        if (itemData == null) itemData = EventSystem.current.currentSelectedGameObject.GetComponent<ItemController>().itemData;
        displayedItem = itemData.name;
        nameText.text = itemData.name;
        descriptionText.text = itemData.description;
    }

    public void SelectItem()
    {
        GameData.escapeRoomGameplayManagerScript.selectedItem = displayedItem;
    }
}
