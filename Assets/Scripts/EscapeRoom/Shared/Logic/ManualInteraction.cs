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

    [Header("[DATA]")]
    [SerializeField] private ItemData itemData;

    void Start()
    {
        // TODO: EVERYONE SHOULD HAVE ITEMDATA EVENTUALLY - PUZZLES AND ZOOMS DO NOT ATM
        itemData = GetComponent<ItemController>()?.itemData;
    }

    public void ItemInteraction()
    {
        // TODO: EVERYONE SHOULD HAVE ITEMDATA EVENTUALLY - PUZZLES AND ZOOMS DO NOT ATM
        GameData.escapeRoomGameplayManagerScript.interactingWith = gameObject.name;

        if (!string.IsNullOrEmpty(GameData.escapeRoomGameplayManagerScript.selectedItem) && !string.IsNullOrEmpty(GameData.escapeRoomGameplayManagerScript.interactingWith))
        {
            GameData.escapeRoomGameplayManagerScript.UseItem(GameData.escapeRoomGameplayManagerScript.selectedItem, GameData.escapeRoomGameplayManagerScript.interactingWith);
        }

        // flag check: if the current flag is true, update dialogue to be the next one possible
        while (dialoguesIndex < activatingFlags.Count && GameProgression.GameProgressionInstance.GetFlag(activatingFlags[dialoguesIndex]))
        {
            dialoguesIndex++;
        }
        
        if (dialoguesIndex < triggeringFlags.Count + eventOffset
            && ((GameData.escapeRoomNumber == 0 
                && (gameObject.name.Equals("LightSwitch") 
                || gameObject.name.Equals("Lever") 
                || (gameObject.name.Equals("LightSwitchPuzzle") && GameProgression.GameProgressionInstance.GetFlag("usedLever")) 
                || GameProgression.GameProgressionInstance.GetFlag("solvedLightSwitchPuzzle")))
            || (GameData.escapeRoomNumber == 1)
            || (GameData.escapeRoomNumber == 2)
            || (GameData.escapeRoomNumber == 3)
            || (GameData.escapeRoomNumber == 4)))
        {
            GameProgression.GameProgressionInstance.SetFlag(triggeringFlags[dialoguesIndex - eventOffset], true);
        }

        GameProgression.GameProgressionInstance.ShowDialogue(characterDialogues[dialoguesIndex]);
    }
}
