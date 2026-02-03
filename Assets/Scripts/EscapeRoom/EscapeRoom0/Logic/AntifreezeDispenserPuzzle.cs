using UnityEngine;
using UnityEngine.EventSystems;

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
        }

        public void OnLetterClick()
        {
            if (currentState == State.Idle)
            {
                GameObject button = EventSystem.current.currentSelectedGameObject;
                selectedLetterIndex = button.transform.GetSiblingIndex();
                Debug.Log(selectedLetterIndex);
            }
            // If Idle: Highlight letter
            // If LetterSelected: Swap letters
        }

        protected override void SolvedPuzzleSpecific()
        {
            
        }
    }
}