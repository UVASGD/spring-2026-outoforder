using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EscapeRoom.EscapeRoom0.Logic
{
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
            guess = new char[] { 'F', 'A', 'R', 'E', 'Z', 'E', 'N', 'I', 'E', 'T'};
            answer = new char[] { 'A', 'N', 'T', 'I', 'F', 'R', 'E', 'E', 'Z', 'E'};
            
            // Initialize button texts
            for (int i = 0; i < guess.Length; i++)
            {
                TextMeshProUGUI text = transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
                text.text = guess[i].ToString();
            }
        }

        public void OnLetterClick()
        {
            // If Idle: Highlight letter
            // If LetterSelected: Swap letters
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
                
                // Swap letters in guess
                (guess[selectedLetterIndex], guess[newLetterIndex]) = (guess[newLetterIndex], guess[selectedLetterIndex]);
                
                
                // Update button texts
                TextMeshProUGUI selectedLetterTMP = transform.GetChild(selectedLetterIndex).GetComponentInChildren<TextMeshProUGUI>();
                TextMeshProUGUI newLetterTMP = newButton.GetComponentInChildren<TextMeshProUGUI>();
                selectedLetterTMP.text = guess[selectedLetterIndex].ToString();
                newLetterTMP.text = guess[newLetterIndex].ToString();
                
                // Unhighlight previously selected letter
                GameObject previousButton = transform.GetChild(selectedLetterIndex).gameObject;
                previousButton.GetComponent<Image>().color = Color.white;

                currentState = State.Idle;
            }
        }

        protected override void SolvedPuzzleSpecific()
        {
            
        }
    }
}