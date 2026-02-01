namespace EscapeRoom.EscapeRoom0.Logic
{
    public class AntifreezeDispenser : Puzzle
    {
        private enum State
        {
            Idle, LetterSelected
        }

        private State currentState = State.Idle;
        
        void Awake()
        {
            guess = new char[] { 'F', 'A', 'R', 'E', 'Z', 'E', 'N', 'I', 'E', 'T'};
            answer = new char[] { 'A', 'N', 'T', 'I', 'F', 'R', 'E', 'E', 'Z', 'E'};
        }

        void OnLetterClick()
        {
            // If Idle: Highlight letter
            // If LetterSelected: Swap letters
        }

        protected override void SolvedPuzzleSpecific()
        {
            
        }
    }
}