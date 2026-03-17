using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public bool scrollView;
    public ItemData itemData;

    void Start()
    {
        UpdateItemData();
    }

    public void UpdateItemData()
    {
        itemData = GameData.escapeRoomGameplayManager.items[gameObject.name];

        if (scrollView)
        {
            GetComponent<Button>().onClick.AddListener(() => GetComponentInParent<ItemPopUp>().ShowItem());
        }
        else
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;   
        }
    }
}
