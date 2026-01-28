using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using System.Globalization;
using UnityEngine;

public class LightSwitchPuzzle : Puzzle
{
    void Awake()
    {
        answer = new char[] { '9', '0', '6', '0', '1', '2', '0' };
        guess = new char[] { '0', '0', '0', '0', '0', '0', '0' };
    }

    public void IncrementDigit()
    {
        GameObject digit = EventSystem.current.currentSelectedGameObject;
        TextMeshProUGUI digitTMP = digit.GetComponentInChildren<TextMeshProUGUI>();
        int currentDigit = int.Parse(digitTMP.text);

        if (currentDigit == 9)
        {
            currentDigit = 0;
        }
        else
        {
            currentDigit++;
        }

        digitTMP.text = currentDigit.ToString();
        guess[digit.transform.GetSiblingIndex()] = (char)('0' + currentDigit);
    }
}
