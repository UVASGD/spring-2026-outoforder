using UnityEngine;
using UnityEngine.UI;

public class PixelShift : MonoBehaviour
{
    private bool fileVisible;

    void Awake()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<Button>().enabled = false;
    }

    void Update()
    {
        if (!fileVisible && GameProgression.GameProgressionInstance.GetFlag("usedFlashDrive"))
        {
            fileVisible = true;
            GetComponent<Image>().enabled = true;
            GetComponent<Button>().enabled = true;
        }
    }

    public void OpenFile()
    {
        GameObject pixelShiftPuzzle = GameData.escapeRoomGameplayManagerScript.puzzles["PixelShiftPuzzle"];
        pixelShiftPuzzle.SetActive(true);
        pixelShiftPuzzle.GetComponent<ManualInteraction>().ItemInteraction();
        gameObject.SetActive(false);
    }
}
