using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LaptopPasswordPuzzle : Puzzle
{
    [SerializeField] TMP_InputField inputField;

    private GameObject keypad;

    void Awake()
    {
        answer = new char[] { '4', '2', '1', '2' };

        keypad = GameObject.Find("Keypad");
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["LaptopPasswordPuzzleSecondary"];

        keypad.SetActive(false);

        // // TODO: THIS COULD PROBABLY BE A METHOD IN PUZZLE THIS CODE IS REPEATED A LOT
        GameProgression.GameProgressionInstance.SetFlag("firstInteractionLaptopPasswordPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedLaptopPasswordPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }

    public void OnKeypadClick()
    {
        var button = EventSystem.current.currentSelectedGameObject; 
        var num = button.transform.GetSiblingIndex() + 1;

        if (num == 10)
        {
            if (inputField.text.Length > 0)
            // Remove last character
            inputField.text = inputField.text[..^1];
        }
        else
        {
            inputField.text += num.ToString();
        }

        guess = inputField.text.ToCharArray();
    }
}