using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorBeltPuzzle : Puzzle
{
    public GameObject submit;
    private readonly int CODE_LENGTH = 4;

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

        submit = transform.Find("Submit").gameObject;
    }
    protected override void SolvedPuzzleSpecific()
    {
        GameProgression.GameProgressionInstance.SetFlag("firstInteractionConveyorBeltPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedConveyorBeltPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();

        ShowRCCarParts();
    }

    public void DisplayPartNames()
    {
        bool allPartsAreEqual = true;
        // Assumes part names are in the second-to-last object.
        foreach (Transform child in transform.GetChild(transform.childCount - 2))
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
        yield return new WaitForSeconds(3.00f);
        // Assumes part names are in the second-to-last object.
        foreach (Transform child in transform.GetChild(transform.childCount - 2))
        {
            child.gameObject.SetActive(false);
        }
    }

    private bool PartCodesAreEqual(int partIndex)
    {
        int startingIndex = partIndex * CODE_LENGTH;

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

    private void ShowRCCarParts()
    {
        // Assumes parts are in the last object.
        foreach (Transform child in transform.GetChild(transform.childCount - 1))
        {
            child.gameObject.SetActive(true);
        }
    }
}