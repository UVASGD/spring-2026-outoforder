using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ConveyorBeltPuzzle : Puzzle
{
    public GameObject partNames;
    private GameObject parts;

    void Awake()
    {
        answer = new char[] 
        { 
            '1', '5', '4', '2', 
            '4', '2', '5', '3',
            '2', '4', '3', '1'
        };

        guess = new char[]
        {
            '0', '0', '0', '0',
            '0', '0', '0', '0',
            '0', '0', '0', '0'
        };

        partNames = transform.Find("PartNames").gameObject;

        parts = transform.Find("Parts").gameObject;
        parts.SetActive(false);
    }
    
    protected override void SolvedPuzzleSpecific()
    {
        parts.SetActive(true);

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionConveyorBeltPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedConveyorBeltPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }

    public void DisplayPartNames()
    {
        bool allPartsAreEqual = true;

        foreach (Transform child in partNames.transform)
        {
            child.gameObject.SetActive(true);

            if (PartCodesAreEqual(child.GetSiblingIndex()))
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = child.name; 
            }
            else
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = "---";
                allPartsAreEqual = false;
            }
        }

        if (!allPartsAreEqual)
        {
            StartCoroutine(StopDisplayingPartNames());
        }
    }

    private IEnumerator StopDisplayingPartNames()
    {
        yield return new WaitForSeconds(3f);
        
        foreach (Transform child in partNames.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private bool PartCodesAreEqual(int partIndex)
    {
        int startingIndex = partIndex * 4;

        char[] answerCode =
        {
            answer[startingIndex], answer[startingIndex + 1],
            answer[startingIndex + 2], answer[startingIndex + 3]
        };
        char[] guessCode = {
            guess[startingIndex], guess[startingIndex + 1],
            guess[startingIndex + 2], guess[startingIndex + 3]
        };

        if (answerCode.SequenceEqual(guessCode))
        {
            return true;
        }

        return false;
    }
}