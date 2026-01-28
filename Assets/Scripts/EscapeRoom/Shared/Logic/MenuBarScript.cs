using UnityEngine;
using UnityEngine.UI;

public class MenuBarScript : MonoBehaviour
{
    [SerializeField] private string currentMenuName = "none";
    private GameObject currentPopUp;
    private GameObject menuBarButtons;
    private Button logButton;
    private Button moveButton;
    private Button itemButton;
    private Button infoButton;
    private GameObject popUpAreas;
    private GameObject logPopUp;
    private GameObject movePopUp;
    private GameObject itemPopUp;
    private GameObject infoPopUp;

    void Start()
    {
        menuBarButtons = transform.Find("MenuBarButtons").gameObject;
        logButton = menuBarButtons.transform.Find("LogButton").GetComponent<Button>();
        moveButton = menuBarButtons.transform.Find("MoveButton").GetComponent<Button>();
        itemButton = menuBarButtons.transform.Find("ItemButton").GetComponent<Button>();
        infoButton = menuBarButtons.transform.Find("InfoButton").GetComponent<Button>();
    
        popUpAreas = transform.Find("PopUpAreas").gameObject;
        logPopUp = popUpAreas.transform.Find("LogPopUp").gameObject;
        movePopUp = popUpAreas.transform.Find("MovePopUp").gameObject;
        itemPopUp = popUpAreas.transform.Find("ItemPopUp").gameObject;
        infoPopUp = popUpAreas.transform.Find("InfoPopUp").gameObject;
    }

    void Update()
    {
        
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
    }

    private void ShowPopUp(GameObject popUp, string newMenuName)
    {
        currentMenuName = newMenuName;
        currentPopUp?.SetActive(false);
        popUp.SetActive(true);
        currentPopUp = popUp;
    }
}
