using System.Collections.Generic;
using UnityEngine;

public class ManualInteraction : MonoBehaviour
{
    [Header("[FLAGS]")]
    [SerializeField] private List<string> activatingFlags;
    [SerializeField] private List<string> triggeringFlags;

    [Header("[DIALOGUE]")]
    [SerializeField] private int characterDialoguesIndex;
    [SerializeField] private List<TextAsset> characterDialogues;

    public void ItemInteraction()
    {
        // flag check
        // if the current flag is true, update dialogue to be the next one possible
        while (characterDialoguesIndex < activatingFlags.Count && GameProgression.GameProgressionInstance.GetFlag(activatingFlags[characterDialoguesIndex]))
        {
            characterDialoguesIndex++;
        }
        
        if (characterDialoguesIndex < triggeringFlags.Count) GameProgression.GameProgressionInstance.SetFlag(triggeringFlags[characterDialoguesIndex], true);

        GameProgression.GameProgressionInstance.ShowDialogue(characterDialogues[characterDialoguesIndex]);
    }
}
