using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace EscapeRoom.EscapeRoom2.Logic
{
    public class CameraFeedPuzzle : Puzzle
    {
        private int[] coords = { 0, 0 };
        private List<(int, int)> targets = new(){ (1, 2), (3, 4), (5, 6) };
        private int maxMoves = 0;

        private int currentMoves = 0;
        
        
        void Awake()
        {
            answer = new[] {'1', '1', '1'};
            guess = new[] {'0', '0', '0'};
        }

        public void OnControlClick()
        {
            var button = EventSystem.current.currentSelectedGameObject; 
            var num = button.transform.GetSiblingIndex() + 1;

            switch (num)
            {
                case 1:
                    
            }
        }
    }
}