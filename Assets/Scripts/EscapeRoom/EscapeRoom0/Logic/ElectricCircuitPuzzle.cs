using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElectricCircuitPuzzle : Puzzle
{
    private TMP_InputField inputField;
    private Image finalLight;
    private Image firstSlotImage;
    private Image secondSlotImage;
    private Image currentSlotImage;
    private Button firstSlot;
    private Button secondSlot;
    private Button currentSlot;
    private char? firstGateCode = null;
    private char? secondGateCode = null;
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

    void Awake()
    {
        inputField = GameObject.Find("InputField").gameObject.GetComponent<TMP_InputField>();
        finalLight = GameObject.Find("FinalLight").gameObject.GetComponent<Image>();
        firstSlotImage = GameObject.Find("FirstSlot").gameObject.GetComponent<Image>();
        secondSlotImage = GameObject.Find("SecondSlot").gameObject.GetComponent<Image>();
        firstSlot = GameObject.Find("FirstSlot").gameObject.GetComponent<Button>();
        secondSlot = GameObject.Find("SecondSlot").gameObject.GetComponent<Button>();

        inputField.gameObject.SetActive(false);
        inputField.onSubmit.AddListener(OnTextEntered);
        finalLight.enabled = false;
    }

    public void OnFirstSlotClicked()
    {
        if (currentSlotImage != null) currentSlotImage.color = Color.white;
        currentSlotImage = firstSlotImage;
        OpenInputField(firstSlot);
    }

    public void OnSecondSlotClicked()
    {
        if (currentSlotImage != null) currentSlotImage.color = Color.white;
        currentSlotImage = secondSlotImage;
        OpenInputField(secondSlot);
    }

    private void OpenInputField(Button slotButton)
    {
        currentSlotImage.color = Color.yellow;
        currentSlot = slotButton;
        inputField.text = "";
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
        inputField.placeholder.GetComponent<TMP_Text>().text = "LOGIC GATE TYPE";
    }

    private void OnTextEntered(string text)
    {
        currentSlotImage.color = Color.white;

        text = text.Trim().ToUpper();
        print("Input " + text);

        if (!((currentSlot == firstSlot) ? firstSlotValidGate : secondSlotValidGate).ContainsKey(text))
        {
            print("TODO: ERROR UI");
            inputField.text = "";
            return;
        }
        if (currentSlot == firstSlot) firstGateCode = firstSlotValidGate[text]; else secondGateCode = secondSlotValidGate[text];

        inputField.gameObject.SetActive(false);
        currentSlot = null;

        EvaluateCircuit();
    }


    private void EvaluateCircuit()
    {
        print("evaluating");
        
        if (firstGateCode == null || secondGateCode == null)
        {
            print("One or more gates aren't set");
            return;
        }

        bool firstResult = firstGate(firstGateCode.Value, true, true);
        bool finalResult = secondGate(secondGateCode.Value, firstResult);

        print($"First gate {firstResult}, Final result {finalResult}");

        finalLight.enabled = finalResult;
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
