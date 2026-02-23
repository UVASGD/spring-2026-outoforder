using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EscapeRoom.EscapeRoom2.Logic
{
    public class CameraFeedPuzzle : Puzzle
    {
        private Vector2 initCursorPos = new(0, 0);
        private Vector2 offset = new(1, 1);
        [SerializeField] private GameObject grid;
        [SerializeField] private Vector2 gridSize = new(5, 5);
        
        [SerializeField] private GameObject cursor;
        private Vector2 coords = new(0, 0);
        [SerializeField] private List<Vector2> targets = new(){ new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6) };
        private int hitTargets = 0;
        
        [SerializeField] private int maxMoves = 7;
        private int currentMoves = 0;
        [SerializeField] TextMeshProUGUI movesText;
        
        
        void Awake()
        {
            answer = new[] {'1', '1', '1'};
            guess = new[] {'0', '0', '0'};
            initCursorPos = cursor.transform.position;
            RectTransform t = (RectTransform) grid.transform;
            offset = new Vector2(t.rect.width / gridSize.x, -t.rect.height / gridSize.y);
            UpdateText();
        }

        public void OnControlClick()
        {
            if (currentMoves >= maxMoves) return;
            var button = EventSystem.current.currentSelectedGameObject; 
            var num = button.transform.GetSiblingIndex() + 1;

            switch (num)
            {
                case 1:
                    // up
                    coords[1] -= hitTargets + 1;
                    break;
                case 2:
                    // down
                    coords[1] += hitTargets + 1;
                    break;
                case 3:
                    // left
                    coords[0] -= hitTargets + 1;
                    break;
                case 4:
                    // right
                    coords[0] += hitTargets + 1;
                    break;
            }
            MoveCursor();
            currentMoves++;
            UpdateText();
        }

        private void MoveCursor()
        {
            if (coords.x < 0) coords.x = 0;
            if (coords.y < 0) coords.y = 0;
            if (coords.x >= gridSize.x) coords.x = gridSize.x - 1;
            if (coords.y >= gridSize.y) coords.y = gridSize.y - 1;
            cursor.transform.position = initCursorPos + offset * coords;

            if (targets.Contains(coords))
            {
                // which one
                int index = targets.IndexOf(coords);
                guess[index] = '1';
                hitTargets++;
                grid.transform.GetChild(index).GetComponent<Image>().color = Color.black;
            }
        }

        public void ResetGrid()
        {
            // Reset all grid dots
            for (int i = 0; i < grid.transform.childCount; i++)
            {
                grid.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
            }
            coords = new Vector2(0, 0);
            MoveCursor();
            currentMoves = 0;
            hitTargets = 0;
            guess = new[] { '0', '0', '0' };
            UpdateText();
        }

        void UpdateText()
        {
            movesText.text = $"Moves: {currentMoves}/{maxMoves}";
            if (currentMoves == maxMoves)
            {
                movesText.color = Color.red;
            }
            else
            {
                movesText.color = Color.black;
            }
        }
    }
}