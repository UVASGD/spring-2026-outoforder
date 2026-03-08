using UnityEngine;

public class PixelShiftPuzzle : Puzzle
{
    void Awake()
    {
        answer = new char[] 
            { 
                '0', '1', '2',
                '3', '4', '5',
                '6', '7', '8'
            };
        guess = new char[]
            { 
                '0', '1', '2',
                'A', 'I', 'Z',
                'A', 'I', 'Z'
            };
    }

    void Update()
    {
        
    }
}
