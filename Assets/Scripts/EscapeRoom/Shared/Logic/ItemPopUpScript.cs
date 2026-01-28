using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPopUpScript : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;

    void Start()
    {
        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        descriptionText = transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
    }

    public void ShowItem()
    {
        ItemData itemData = EventSystem.current.currentSelectedGameObject.GetComponent<ItemController>().itemData;
        nameText.text = itemData.name;
        descriptionText.text = itemData.description;
    }
}
