using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElectricCircuitPuzzle : Puzzle
{
    public TMP_InputField inputField;
    public GameObject finalLight;

    public Button firstButton;
    public Button secondButton;

    private char? firstGateCode = null;
    private char? secondGateCode = null;

    private Button currentButton;

    private Dictionary<string, char> firstSlotValidGate = new Dictionary<string, char>
    {
        { "AND",  '0' },
        { "OR",   '1' },
        { "NAND", '2' },
        { "NOR",  '3' },
        { "XOR",  '4' },
        { "XNOR", '5' }
    };


    private Dictionary<string, char> secondSlotValidGate = new Dictionary<string, char>
    {
        { "NOT", '6' }
    };


    void Start()
    {
        inputField.gameObject.SetActive(false);

        if (finalLight != null)
        {
            finalLight.SetActive(false);
        }
        
        inputField.onEndEdit.AddListener(OnTextEntered);
    }

    public void OnFirstSlotClicked()
    {
        OpenInputField(firstButton);
        print("First slot was clicked");
    }

    public void OnSecondSlotClicked()
    {
        OpenInputField(secondButton);
        print("Second slot was clicked");
    }

    private void OpenInputField(Button slotButton)
    {
        currentButton = slotButton;
        inputField.text = "";
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
        inputField.placeholder.GetComponent<TMP_Text>().text = "LOGIC GATE TYPE (BASIC ONLY)";
    }

    private void OnTextEntered(string text)
    {
        if (currentButton == null)
        {
            // Keeps getting stuck on this line
            print("Input entered but no slot selected!");
            return;
        }

        text = text.Trim().ToUpper();
        print("Input " + text);

        if (currentButton == firstButton)
        {
            if (!firstSlotValidGate.TryGetValue(text, out char code))
            {
                inputField.text = "";
                return;
            }
            firstGateCode = code;
        }
        else if (currentButton == secondButton)
        {
            if (!secondSlotValidGate.TryGetValue(text, out char code))
            {
                inputField.text = "";
                return;
            }
            secondGateCode = code;
        }

        inputField.gameObject.SetActive(false);
        currentButton = null;

        EvaluateCircuit();
    }


    private void EvaluateCircuit()
    {
        if (firstGateCode == null || secondGateCode == null)
        {
            print("One or more gates aren't set");
            finalLight.SetActive(false);
            return;
        }

        bool firstResult = firstGate(firstGateCode.Value, true, true);
        bool finalResult = secondGate(secondGateCode.Value, firstResult);

        print($"First gate {firstResult}, Final result {finalResult}");

        finalLight.SetActive(finalResult);
    }


    private bool firstGate(char gate, bool a, bool b)
    {
        return gate switch
        {
            '0' => a & b,
            '1' => a | b,
            '2' => !(a & b),
            '3' => !(a | b),
            '4' => a ^ b,
            '5' => !(a ^ b),
            _ => false
        };
    }

    private bool secondGate(char gate, bool a)
    {
        return gate == '6' ? !a : false;
    }
}
