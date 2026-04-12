using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public void ExecuteLog()
    {
        print("log");
    }

    public void ExecuteAuto()
    {
        print("auto");
        GameData.autoDialogueProgression = !GameData.autoDialogueProgression;
        TextMeshProUGUI buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = buttonText.text.Equals("auto")
            ? "manual"
            : "auto";
    }

    public void ExecuteSave()
    {
        
    }


    public void ExecuteLoad()
    {
        
    }
}
