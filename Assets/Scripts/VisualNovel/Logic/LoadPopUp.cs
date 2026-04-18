using UnityEngine;

public class LoadPopUp : MonoBehaviour
{
    private GameObject confirmationPopUp;
    private int loadFromSlotNumber;

    void OnEnable()
    {
        if (confirmationPopUp == null)
        {
            confirmationPopUp = transform.Find("ConfirmationPopUp").gameObject;
        }
        confirmationPopUp.SetActive(false);
    }

    public void LoadFromSlot(int slotNumber)
    {
        loadFromSlotNumber = slotNumber;
        confirmationPopUp.SetActive(true);
    }

    public void Cancel()
    {
        confirmationPopUp.SetActive(false);
    }

    public void Load()
    {
        print($"loaded from {loadFromSlotNumber}");

        GameProgression.GameProgressionInstance.Load(loadFromSlotNumber);

        confirmationPopUp.SetActive(false);
    }
}
