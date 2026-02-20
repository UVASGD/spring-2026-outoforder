using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ConveyorBeltPuzzle : Puzzle
{
    public class ValidCodeComparer : IEqualityComparer<char[]>
    {
        public bool Equals(char[] x, char[] y) => x.SequenceEqual(y);

        public int GetHashCode(char[] obj)
        {
            HashCode hash = new HashCode();
            foreach (char c in obj) hash.Add(c);
            return hash.ToHashCode();
        }
    }
    public GameObject partNames;
    private GameObject parts;
    private Coroutine clearDisplay;
    private Dictionary<char[], int> validCodes = new(new ValidCodeComparer()) 
    {
        { new[] { '1', '5', '4', '2' }, 0 },
        { new[] { '4', '2', '5', '3' }, 1 },
        { new[] { '2', '4', '3', '1' }, 2 }
    };
    private char[] resetValue = new char[] 
        { 
            '0', '0', '0', '0', 
            '0', '0', '0', '0',
            '0', '0', '0', '0' 
        };
    private char[] temporaryGuess = new char[12];
    private char[] actualValues = new char[12];
    
    void Awake()
    {
        answer = new char[12] 
            { 
                '1', '1', '1', '1', 
                '1', '1', '1', '1', 
                '1', '1', '1', '1' 
            };
        guess = (char[])resetValue.Clone();

        partNames = transform.Find("PartNames").gameObject;

        parts = transform.Find("Parts").gameObject;
        parts.SetActive(false);

        temporaryGuess = (char[])resetValue.Clone();
        actualValues = (char[])resetValue.Clone();
    }
    
    protected override void SolvedPuzzleSpecific()
    {
        parts.SetActive(true);

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionConveyorBeltPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedConveyorBeltPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }

    public void DetermineParts()
    {
        HashSet<int> indices = new() { 0, 1, 2 };
        
        foreach (Transform child in partNames.transform)
        {
            child.gameObject.SetActive(true);

            // TODO: shown name is not complete and should be determined in PartCodesAreEqual as a return value of string probably
            if (PartCodesAreEqual(child.GetSiblingIndex(), indices))
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = child.name;
            }
            else
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().text = "---";
            }
        }

        guess = (char[])temporaryGuess.Clone();

        if (indices.Count != 0)
        {
            if (clearDisplay != null)
            {
                StopCoroutine(clearDisplay);
                
                clearDisplay = null; 
            }

            clearDisplay = StartCoroutine(ClearDisplay());
        }
    }

    private IEnumerator ClearDisplay()
    {
        yield return new WaitForSeconds(3f);
        
        foreach (Transform child in partNames.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ResetGuess()
    {
        guess = (char[])actualValues.Clone();
    }

    private bool PartCodesAreEqual(int partIndex, HashSet<int> indices)
    {
        int startingIndex = partIndex * 4;
        char[] currentGuess = new char[4];
        Array.Copy(guess, startingIndex, currentGuess, 0, 4);

        actualValues[startingIndex]     = guess[startingIndex];
        actualValues[startingIndex + 1] = guess[startingIndex + 1];
        actualValues[startingIndex + 2] = guess[startingIndex + 2];
        actualValues[startingIndex + 3] = guess[startingIndex + 3];
    
        bool isValidCode = validCodes.ContainsKey(currentGuess);

        if (isValidCode)
        {
            int index = validCodes[currentGuess];

            if (indices.Contains(index))
            {
                index *= 4;

                temporaryGuess[index]     = '1';
                temporaryGuess[index + 1] = '1';
                temporaryGuess[index + 2] = '1';
                temporaryGuess[index + 3] = '1';

                indices.Remove((char)index);
           }
        }

        return isValidCode;
    }
}