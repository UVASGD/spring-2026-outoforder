using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle : MonoBehaviour
{
    protected char[] answer;
    protected char[] guess;
    protected bool solved;

    public void AttemptSolve()
    {
        print($"the answer is {new string(answer)} and the guess was {new string(guess)}");
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
    public void IncrementLetter()
    {
        if (!solved)
        {
            GameObject character = EventSystem.current.currentSelectedGameObject;
            TextMeshProUGUI letterTMP = character.GetComponentInChildren<TextMeshProUGUI>();

            char currentChar = letterTMP.text[0];

            // Force uppercase
            if (currentChar < 'A' || currentChar > 'Z')
            {
                currentChar = 'A';
            }

            char nextChar = (char)('A' + (currentChar - 'A' + 1) % 26);

            letterTMP.text = nextChar.ToString();
            guess[character.transform.GetSiblingIndex()] = nextChar;
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
        print($"TODO: ERROR GUESS UI; the guess was {new string(guess)}");
    }

    protected virtual void SolvedPuzzleSpecific() {}
}
