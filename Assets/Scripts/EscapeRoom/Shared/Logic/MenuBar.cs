using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    [SerializeField] private string currentMenuName = "none";
    private GameObject currentPopUp;
    private GameObject interactionBlocker;
    private GameObject popUpAreas;
    private GameObject logPopUp;
    private GameObject movePopUp;
    private GameObject itemPopUp;
    private GameObject infoPopUp;
    private Button moveButton;

    void Awake()
    {
        GameData.escapeRoomGameplayManager.menuBar = this;
    }

    void Start()
    {
        interactionBlocker = transform.Find("InteractionBlocker").gameObject;
        interactionBlocker.SetActive(false);

        popUpAreas = transform.Find("PopUpAreas").gameObject;
        logPopUp = popUpAreas.transform.Find("LogPopUp").gameObject;
        movePopUp = popUpAreas.transform.Find("MovePopUp").gameObject;
        itemPopUp = popUpAreas.transform.Find("ItemPopUp").gameObject;
        infoPopUp = popUpAreas.transform.Find("InfoPopUp").gameObject;

        moveButton = transform.Find("MenuBarButtons").Find("MoveButton").GetComponent<Button>();
    }

    void Update()
    {
        moveButton.interactable = !GameData.escapeRoomGameplayManager.enteredItem;

        if (Input.GetMouseButtonDown(1) && !currentMenuName.Equals("none")) HidePopUp();
    } 

    public void ChangeState(string newMenuName)
    {
        if (currentMenuName == newMenuName)
        {
            HidePopUp();   
        }
        else
        {
            switch (newMenuName)
            {
                case "log":
                    ShowPopUp(logPopUp, newMenuName);
                    break;
                case "move":
                    ShowPopUp(movePopUp, newMenuName);
                    break;
                case "item":
                    ShowPopUp(itemPopUp, newMenuName);
                    break;
                case "info":
                    ShowPopUp(infoPopUp, newMenuName);
                    break;
            }  
        }   
    }

    public void HidePopUp()
    {
        currentMenuName = "none";
        currentPopUp.SetActive(false);
        currentPopUp = null;
        interactionBlocker.SetActive(false);
        GameProgression.GameProgressionInstance.PlaySFX(2);
    }

    private void ShowPopUp(GameObject popUp, string newMenuName)
    {
        currentMenuName = newMenuName;
        currentPopUp?.SetActive(false);
        popUp.SetActive(true);
        currentPopUp = popUp;
        interactionBlocker.SetActive(true);
        GameProgression.GameProgressionInstance.PlaySFX(1);
    }
}
