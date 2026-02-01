using System.Collections.Generic;
using UnityEngine;

public class ManualInteraction : MonoBehaviour
{
    [Header("[FLAGS]")]
    [SerializeField] private List<string> activatingFlags;
    [SerializeField] private List<string> triggeringFlags;

    [Header("[DIALOGUE]")]
    [SerializeField] private int dialoguesIndex;
    [SerializeField] private List<TextAsset> characterDialogues;

    public void ItemInteraction()
    {
        // flag check
        // if the current flag is true, update dialogue to be the next one possible
        while (dialoguesIndex < activatingFlags.Count && GameProgression.GameProgressionInstance.GetFlag(activatingFlags[dialoguesIndex]) 
            && (GameData.escapeRoomNumber == 0 && GameProgression.GameProgressionInstance.GetFlag("solvedLightSwitchPuzzle")))
        {
            print("incremented index");
            dialoguesIndex++;
        }
        
        if (dialoguesIndex < triggeringFlags.Count) GameProgression.GameProgressionInstance.SetFlag(triggeringFlags[dialoguesIndex], true);

        GameProgression.GameProgressionInstance.ShowDialogue(characterDialogues[dialoguesIndex]);
    }
}
