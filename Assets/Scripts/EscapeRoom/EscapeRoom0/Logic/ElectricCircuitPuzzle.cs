using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.EventSystems;

public class ElectricCircuitPuzzle : Puzzle
{
    private bool placedDullCyanCore;
    private Image firstSlotImage;
    private Image secondSlotImage;
    private Image cyanCoreImage;
    private SortedDictionary<int, string> firstSlotValidGates = new()
    {
        { 0, "GateAnd" },
        { 1, "GateOr" },
        { 2, "GateNand" },
        { 3, "GateNor" },
        { 4, "GateXor" },
        { 5, "GateXnor" },
        { 6, "Transparent" }
    };
    private SortedDictionary<int, string> secondSlotValidGates = new()
    {
        { 6, "GateNot" },
        { 7, "Transparent" }
    };

    void Awake()
    {
        // TODO: XOR AND NOT IS ALSO CORRECT
        answer = new char[] { '2', '6' };
        guess = new char[] { '6', '7' };

        firstSlotImage = transform.Find("FirstSlot").GetComponent<Image>();
        secondSlotImage = transform.Find("SecondSlot").GetComponent<Image>();
        cyanCoreImage = transform.Find("DullCyanCore").GetComponent<Image>();
    }

    void OnEnable()
    {
        if (GameData.escapeRoomGameplayManager.items["DullCyanCore"].collectible && GameData.escapeRoomGameplayManager.collectedItemsScrollView.Keys.Contains("Dull Cyan Core")) GameData.escapeRoomGameplayManager.items["DullCyanCore"].collectible = false;
    }

    void Update()
    {
        if (!placedDullCyanCore && GameProgression.GameProgressionInstance.GetFlag("usedDullCyanCore"))
        {
            placedDullCyanCore = true;
            cyanCoreImage.enabled = true;
            cyanCoreImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CyanCore"];
        }
    }

    public void AdjustGate(int direction)
    {
        if (!placedDullCyanCore || solved) return;

        GameProgression.GameProgressionInstance.PlaySFX(direction == 1 ? 5 : 6);

        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        
        bool isFirst = buttonName.Contains("First");
        int index = isFirst ? 0 : 1;
        
        int min = isFirst ? 0 : 6;
        int max = isFirst ? 6 : 7;

        int val = (int)char.GetNumericValue(guess[index]);
        val += direction;

        if (val > max) val = min;
        else if (val < min) val = max;

        var slotImage = isFirst ? firstSlotImage : secondSlotImage;
        var validGates = isFirst ? firstSlotValidGates : secondSlotValidGates;

        slotImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[validGates[val]];
        guess[index] = (char)(val + '0');
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["ElectricCircuitPuzzleSecondary"];
       
        cyanCoreImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CyanCoreSecondary"];
        cyanCoreImage.gameObject.name = "GlowingCyanCore";
        cyanCoreImage.gameObject.GetComponent<ItemController>().itemData = GameData.escapeRoomGameplayManager.items["GlowingCyanCore"];

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionElectricCircuitPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedElectricCircuitPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}
