using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class VisualNovelMenuBar : MonoBehaviour
{
    [SerializeField] private string currentMenuName = "none";
    private GameObject currentPopUp;
    private GameObject interactionBlocker;
    private List<TextMeshProUGUI> autoButtonTMPs;
    private GameObject popUpAreas;
    private GameObject logPopUp;
    private GameObject savePopUp;
    private GameObject loadPopUp;

    void Start()
    {
        interactionBlocker = transform.Find("InteractionBlocker").gameObject;
        interactionBlocker.SetActive(false);

        autoButtonTMPs = GameObject.FindGameObjectsWithTag("AutoButton")
            .Select(obj => obj.GetComponentInChildren<TextMeshProUGUI>())
            .Where(tmp => tmp != null)
            .ToList();
            
        popUpAreas = transform.Find("PopUpAreas").gameObject;
        logPopUp = popUpAreas.transform.Find("LogPopUp").gameObject;
        savePopUp = popUpAreas.transform.Find("SavePopUp").gameObject;
        loadPopUp = popUpAreas.transform.Find("LoadPopUp").gameObject;
    }

    public void ExecuteAuto()
    {
        if (!GameProgression.GameProgressionInstance.transitioning)
        {
            GameData.autoDialogueProgression = !GameData.autoDialogueProgression;
            autoButtonTMPs.ForEach(tmp => tmp.text = tmp.text == "auto" ? "manual" : "auto");   
        }
    }

    public void ExecuteSave()
    {
        
    }


    public void ExecuteLoad()
    {
        
    }

    public void ChangeState(string newMenuName)
    {
        print("trying to change state");
        if (!GameProgression.GameProgressionInstance.transitioning)
        {
            if (currentMenuName == newMenuName)
            {
                HidePopUp();   
            }
            else
            {
                if (!newMenuName.Equals("auto") && GameData.autoDialogueProgression) ExecuteAuto();

                switch (newMenuName)
                {
                    case "log":
                        ShowPopUp(logPopUp, newMenuName);
                        break;
                    case "auto":
                        HidePopUp();
                        break;
                    case "save":
                        ShowPopUp(savePopUp, newMenuName);
                        break;
                    case "load":
                        ShowPopUp(loadPopUp, newMenuName);
                        break;
                }  
            }   
        }
    }

    public void HidePopUp(bool playSfx = true)
    {
        currentMenuName = "none";
        currentPopUp?.SetActive(false);
        currentPopUp = null;
        interactionBlocker.SetActive(false);
        if (playSfx) GameProgression.GameProgressionInstance.PlaySFX(2);
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
