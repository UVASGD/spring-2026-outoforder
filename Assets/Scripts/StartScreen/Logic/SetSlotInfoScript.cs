using TMPro;
using UnityEngine;

public class SetSlotInfo : MonoBehaviour
{
    private TextMeshPro slotInfo; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slotInfo = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void printInfo()
    {
        Debug.Log(slotInfo.text);
    }
}
