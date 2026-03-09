using UnityEngine;

public class PixelShiftPuzzle : Puzzle
{
    private GameObject pixelA;
    private int indexA;
    private GameObject pixelB;
    private int indexB;

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

    public void SwapPixel()
    {
        if (pixelA == null)
        {
            pixelA = gameObject;
            indexA = pixelA.transform.GetSiblingIndex();
        }
        else
        {
            pixelB = gameObject;
            indexB = pixelB.transform.GetSiblingIndex();
            
            Vector2 positionA = pixelA.transform.position;
            pixelA.transform.position = pixelB.transform.position;
            pixelB.transform.position = positionA;

            pixelA.transform.SetSiblingIndex(indexB);
            pixelB.transform.SetSiblingIndex(indexA);
        }
    }
}
