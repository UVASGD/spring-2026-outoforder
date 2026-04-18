using UnityEngine;

public class LoadPopUp : MonoBehaviour
{
    private GameObject confirmationPopUp;
    private int loadFromSlotNumber;

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

    public void LoadFromSlot(int slotNumber)
    {
        print($"trying to load from slot number {slotNumber}");
        loadFromSlotNumber = slotNumber;
        confirmationPopUp.SetActive(true);
    }

    public void Cancel()
    {
        print("cancelled");
        confirmationPopUp.SetActive(false);
    }

    public void Load()
    {
        print($"loaded from {loadFromSlotNumber}");

        GameProgression.GameProgressionInstance.Load(loadFromSlotNumber);

        confirmationPopUp.SetActive(false);
    }
}
