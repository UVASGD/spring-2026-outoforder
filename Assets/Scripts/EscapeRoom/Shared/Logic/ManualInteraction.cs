using System.Collections.Generic;
using UnityEngine;

public class ManualInteraction : MonoBehaviour
{
    [Header("[FLAGS]")]
    [SerializeField] private int eventOffset;
    [SerializeField] private List<string> activatingFlags;
    [SerializeField] private List<string> triggeringFlags;

    [Header("[DIALOGUE]")]
    [SerializeField] private int dialoguesIndex;
    [SerializeField] private List<TextAsset> characterDialogues;

    public void ItemInteraction()
    {
        // flag check: if the current flag is true, update dialogue to be the next one possible
        while (dialoguesIndex < activatingFlags.Count && GameProgression.GameProgressionInstance.GetFlag(activatingFlags[dialoguesIndex]))
        {
            dialoguesIndex++;
        }
        
        if (dialoguesIndex < triggeringFlags.Count + eventOffset
            && ((GameData.escapeRoomNumber == 0 && (gameObject.name.Contains("LightSwitch") || GameProgression.GameProgressionInstance.GetFlag("solvedLightSwitchPuzzle")))
            || (GameData.escapeRoomNumber == 1)
            || (GameData.escapeRoomNumber == 2)
            || (GameData.escapeRoomNumber == 3)
            || (GameData.escapeRoomNumber == 4)))
        {
            GameProgression.GameProgressionInstance.SetFlag(triggeringFlags[dialoguesIndex - eventOffset], true);
        }

        print($"solvedLightSwitchPuzzle: {GameProgression.GameProgressionInstance.GetFlag("solvedLightSwitchPuzzle")}");

        GameProgression.GameProgressionInstance.ShowDialogue(characterDialogues[dialoguesIndex]);
    }
}
