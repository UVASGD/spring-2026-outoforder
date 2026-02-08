using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AntifreezeDispenserPuzzle : Puzzle
{
    private enum State
    {
        Idle, LetterSelected
    }

    private State currentState = State.Idle;
    private int selectedLetterIndex = -1;
    
    void Awake()
    {
        answer = new char[] { 'A', 'N', 'T', 'I', 'F', 'R', 'E', 'E', 'Z', 'E'};
        guess = new char[] { 'F', 'A', 'R', 'E', 'Z', 'E', 'N', 'I', 'E', 'T'};
        
        // initialize button texts
        for (int i = 0; i < guess.Length; i++)
        {
            TextMeshProUGUI text = transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
            text.text = guess[i].ToString();
        }
    }

    public void OnLetterClick()
    {
        // if Idle: Highlight letter
        // if LetterSelected: Swap letters
        if (currentState == State.Idle)
        {
            GameObject button = EventSystem.current.currentSelectedGameObject;
            selectedLetterIndex = button.transform.GetSiblingIndex();
            button.GetComponent<Image>().color = Color.yellow;
            currentState = State.LetterSelected;
        }
        else
        {
            GameObject newButton = EventSystem.current.currentSelectedGameObject;
            int newLetterIndex = newButton.transform.GetSiblingIndex();
            
            // swap letters in guess
            (guess[selectedLetterIndex], guess[newLetterIndex]) = (guess[newLetterIndex], guess[selectedLetterIndex]);
            
            
            // update button texts
            TextMeshProUGUI selectedLetterTMP = transform.GetChild(selectedLetterIndex).GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI newLetterTMP = newButton.GetComponentInChildren<TextMeshProUGUI>();
            selectedLetterTMP.text = guess[selectedLetterIndex].ToString();
            newLetterTMP.text = guess[newLetterIndex].ToString();
            
            // unhighlight previously selected letter
            GameObject previousButton = transform.GetChild(selectedLetterIndex).gameObject;
            previousButton.GetComponent<Image>().color = Color.white;

            currentState = State.Idle;
        }
    }

    protected override void SolvedPuzzleSpecific()
    {
        GameProgression.GameProgressionInstance.SetFlag("firstInteractionAntifreezeDispenserPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedAntifreezeDispenserPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}