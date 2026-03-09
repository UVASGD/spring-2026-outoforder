using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
                '7', '3', '1',
                '4', '8', '6',
                '5', '0', '2'
            };
    }

    protected override void ConvertGuess()
    {
        for (int i = 0; i < gameObject.transform.childCount - 1; i++)
        {
            guess[i] = gameObject.transform.GetChild(i).name[^1];
        }
    }

    protected override void SolvedPuzzleSpecific()
    {
        foreach (Transform child in gameObject.transform) 
        {
            child.gameObject.SetActive(false);
        }

        Image image = gameObject.GetComponent<Image>();
        image.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["PixelShiftPuzzle"];
        Color c = image.color;
        c.a = 1f;
        image.color = c;
        
        // TODO: THIS COULD PROBABLY BE A METHOD IN PUZZLE THIS CODE IS REPEATED A LOT
        GameProgression.GameProgressionInstance.SetFlag("firstInteractionPixelShiftPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedPixelShiftPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }

    public void SwapPixel()
    {
        if (pixelA == null)
        {
            print("A!");
            pixelA = EventSystem.current.currentSelectedGameObject;
            indexA = pixelA.transform.GetSiblingIndex();
        }
        else
        {
            print("B and swap!");
            pixelB = EventSystem.current.currentSelectedGameObject;
            indexB = pixelB.transform.GetSiblingIndex();
            
            Vector2 positionA = pixelA.transform.position;
            pixelA.transform.position = pixelB.transform.position;
            pixelB.transform.position = positionA;

            pixelA.transform.SetSiblingIndex(indexB);
            pixelB.transform.SetSiblingIndex(indexA);

            pixelA = null;
            pixelB = null;
        }
    }
}
