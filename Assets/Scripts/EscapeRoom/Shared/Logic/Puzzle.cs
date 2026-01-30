using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle : MonoBehaviour
{
    protected char[] answer;
    protected char[] guess;

    public void AttemptSolve()
    {
        // print($"the answer is {new string(answer)} and the guess was {new string(guess)}");
        // TODO: VERY TEMPORARY
        if (answer.SequenceEqual(guess)) GameObject.Find("DemoMessage").GetComponent<TextMeshProUGUI>().text = "DEMO MESSAGE: SUCCESS";
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
