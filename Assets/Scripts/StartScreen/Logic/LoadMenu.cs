using TMPro;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    private TextMeshProUGUI slotInfo; 

    public void SetInfo()
    {
        slotInfo = GetComponent<TextMeshProUGUI>();
        // slotInfo.SetText("Escape Room No: " + PlayerPrefs.GetInt("Escape Room Number"));
    }
}
