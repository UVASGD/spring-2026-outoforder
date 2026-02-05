using TMPro;
using UnityEngine;

public class SetSlotInfo : MonoBehaviour
{
    private TextMeshProUGUI slotInfo; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slotInfo = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo()
    {
        slotInfo.SetText("Escape Room No: " + PlayerPrefs.GetInt("Escape Room Number"));
    }
}
