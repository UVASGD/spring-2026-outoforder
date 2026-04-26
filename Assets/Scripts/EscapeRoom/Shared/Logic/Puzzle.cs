using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle : MonoBehaviour
{
    protected char[] answer;
    protected char[] guess;
    protected bool solved;
    private Coroutine checkPuzzleAttemptCoroutine;

    public void AttemptSolve()
    {
        if (checkPuzzleAttemptCoroutine == null)
        {
            checkPuzzleAttemptCoroutine = StartCoroutine(CheckPuzzleAttemptCoroutine());
        }
    }

    public void IncrementDigit()
    {
        if (!solved)
        {
            GameProgression.GameProgressionInstance.PlaySFX(5);

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

    public void DecrementDigit()
    {
        if (!solved)
        {
            GameProgression.GameProgressionInstance.PlaySFX(6);
        }
    }

    public void IncrementLetter()
    {
        if (!solved)
        {
            GameProgression.GameProgressionInstance.PlaySFX(5);

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

    public void DecrementLetter()
    {
        if (!solved)
        {
            GameProgression.GameProgressionInstance.PlaySFX(6);
        } 
    }

    private IEnumerator CheckPuzzleAttemptCoroutine()
    {
        ConvertGuess();
        
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

        GameData.escapeRoomGameplayManager.puzzleFeedback.SetActive(true);

        yield return new WaitForSeconds(1f);

        GameData.escapeRoomGameplayManager.puzzleFeedback.SetActive(false);

        yield return new WaitForSeconds(0.25f);

        if (solved) SolvedPuzzleSpecific();

        checkPuzzleAttemptCoroutine = null;
    }

    private void SolvedPuzzle()
    {
        GameProgression.GameProgressionInstance.PlaySFX(10);
        GameData.escapeRoomGameplayManager.puzzleFeedbackImage.color = Color.cyan;
        GameData.escapeRoomGameplayManager.puzzleFeedbackTMP.text = "success";
        print("TODO: CORRECT GUESS UI");
        solved = true;
    }

    private void ErrorPuzzle()
    {
        GameProgression.GameProgressionInstance.PlaySFX(11);
        GameData.escapeRoomGameplayManager.puzzleFeedbackImage.color = Color.magenta;
        GameData.escapeRoomGameplayManager.puzzleFeedbackTMP.text = "error";
        print($"TODO: ERROR GUESS UI; the guess was {new string(guess)}");
    }

    protected virtual void ConvertGuess() {}

    protected virtual void SolvedPuzzleSpecific() {}
}
