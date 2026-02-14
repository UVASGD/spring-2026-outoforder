using TMPro;
using UnityEngine;

namespace EscapeRoom.EscapeRoom1.Logic
{
    public class LaptopPasswordPuzzle : Puzzle
    {
        [SerializeField] TMP_InputField inputField;
        void Awake()
        {
            answer = new char[] { '4', '2', '1', '0' };
        }

        public void OnEdit()
        {
            guess = inputField.text.ToCharArray();
        }
        
        
    }
}