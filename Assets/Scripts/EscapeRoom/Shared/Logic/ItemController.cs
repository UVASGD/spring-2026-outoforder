using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public bool scrollView;
    public ItemData itemData;

    void Start()
    {
        itemData = GameData.escapeRoomGameplayManagerScript.items[gameObject.name];

        if (scrollView)
        {
            GetComponent<Button>().onClick.AddListener(() => GetComponentInParent<ItemPopUpScript>().ShowItem());
        }
        else
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;   
        }
    }
}
