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
        // flag check: if the current flag is true, update dialogue to be the next one possible
        while (dialoguesIndex < activatingFlags.Count && GameProgression.GameProgressionInstance.GetFlag(activatingFlags[dialoguesIndex]))
        {
            dialoguesIndex++;
        }
        
        if (dialoguesIndex < triggeringFlags.Count
            && (GameData.escapeRoomNumber == 0 && (gameObject.name.Equals("LightSwitch") || GameProgression.GameProgressionInstance.GetFlag("solvedLightSwitchPuzzle"))))
        {
            GameProgression.GameProgressionInstance.SetFlag(triggeringFlags[dialoguesIndex], true);
        }

        GameProgression.GameProgressionInstance.ShowDialogue(characterDialogues[dialoguesIndex]);
    }
}
