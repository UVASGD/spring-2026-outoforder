using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle : MonoBehaviour
{
    protected List<GameObject> combinationSlots;
    protected char[] answer;
    protected char[] guess;
    protected bool solved;

    public void AttemptSolve()
    {
        // print($"the answer is {new string(answer)} and the guess was {new string(guess)}");
        // TODO: VERY TEMPORARY
        if (!solved && answer.SequenceEqual(guess)) 
        {
            SolvedPuzzle();
        }
        else if (!answer.SequenceEqual(guess))
        {
            ErrorPuzzle();
        }
    }

    public void IncrementDigit()
    {
        if (!solved)
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

    private void SolvedPuzzle()
    {
        print("TODO: CORRECT GUESS UI");
        solved = true;
        SolvedPuzzleSpecific();
    }

    private void ErrorPuzzle()
    {
        print("TODO: ERROR GUESS UI");
    }

    protected virtual void SolvedPuzzleSpecific() {}
}
