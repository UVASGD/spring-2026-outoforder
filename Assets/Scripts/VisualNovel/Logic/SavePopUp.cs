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
        print("save is enabled");
        if (confirmationPopUp == null)
        {
            confirmationPopUp = transform.Find("ConfirmationPopUp").gameObject;
        }
        confirmationPopUp.SetActive(false);
    }

    public void SaveToSlot(int slotNumber)
    {
        print($"trying to save to slot number {slotNumber}");
        saveToSlotNumber = slotNumber;
        confirmationPopUp.SetActive(true);
    }

    public void Cancel()
    {
        print("cancelled");
        confirmationPopUp.SetActive(false);
    }

    public void Save()
    {
        print($"saved to {saveToSlotNumber}");

        GameProgression.GameProgressionInstance.Save(saveToSlotNumber);

        confirmationPopUp.SetActive(false);
    }
}
