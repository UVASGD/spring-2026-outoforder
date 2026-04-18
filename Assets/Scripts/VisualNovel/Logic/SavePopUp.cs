using UnityEngine;

public class SavePopUp : MonoBehaviour
{
    private GameObject confirmationPopUp;
    private int saveToSlotNumber;

    void Start()
    {
        
    }

    void OnEnable()
    {
        if (confirmationPopUp == null)
        {
            confirmationPopUp = transform.Find("ConfirmationPopUp").gameObject;
        }
        confirmationPopUp.SetActive(false);
    }

    public void SaveToSlot(int slotNumber)
    {
        saveToSlotNumber = slotNumber;
        confirmationPopUp.SetActive(true);
    }

    public void Cancel()
    {
        confirmationPopUp.SetActive(false);
    }

    public void Save()
    {
        print($"saved to {saveToSlotNumber}");

        GameProgression.GameProgressionInstance.Save(saveToSlotNumber);

        confirmationPopUp.SetActive(false);
    }
}
