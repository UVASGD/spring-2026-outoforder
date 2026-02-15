using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaptopPasswordPuzzle : Puzzle
{
    [SerializeField] TMP_InputField inputField;

    void Awake()
    {
        answer = new char[] { '4', '2', '1', '2' };
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