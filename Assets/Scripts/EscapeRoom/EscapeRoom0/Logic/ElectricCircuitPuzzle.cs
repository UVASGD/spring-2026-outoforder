using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElectricCircuitPuzzle : Puzzle
{
    private bool placedCyanCoreDull;
    private TMP_InputField inputField;
    private Image firstSlotImage;
    private Image secondSlotImage;
    private Image currentSlotImage;
    private Button firstSlotButton;
    private Button secondSlotButton;
    private Button currentSlot;
    private Image cyanCoreImage;
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
        answer = new char[] { '2', '6' };
        guess = new char[] { '7', '7' };

        inputField = transform.Find("InputField").gameObject.GetComponent<TMP_InputField>();
        firstSlotImage = transform.Find("FirstSlot").gameObject.GetComponent<Image>();
        secondSlotImage = transform.Find("SecondSlot").gameObject.GetComponent<Image>();
        firstSlotButton = transform.Find("FirstSlot").gameObject.GetComponent<Button>();
        secondSlotButton = transform.Find("SecondSlot").gameObject.GetComponent<Button>();
        cyanCoreImage =  transform.Find("CyanCore").gameObject.GetComponent<Image>();

        inputField.gameObject.SetActive(false);
        inputField.onSubmit.AddListener(OnTextEntered);
    }

    void Update()
    {
        if (!placedCyanCoreDull && GameProgression.GameProgressionInstance.GetFlag("usedDullCyanCore"))
        {
            placedCyanCoreDull = true;
            cyanCoreImage.enabled = true;
            cyanCoreImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CyanCore"];
        }
    }

    public void OnFirstSlotClicked()
    {
        if (placedCyanCoreDull && !solved)
        {
            if (currentSlotImage != null) currentSlotImage.color = Color.white;
            currentSlotImage = firstSlotImage;
            OpenInputField(firstSlotButton);
        }
    }

    public void OnSecondSlotClicked()
    {
        if (placedCyanCoreDull && !solved)
        {
            if (currentSlotImage != null) currentSlotImage.color = Color.white;
            currentSlotImage = secondSlotImage;
            OpenInputField(secondSlotButton);
        }   
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

        if (!((currentSlot == firstSlotButton) ? firstSlotValidGate : secondSlotValidGate).ContainsKey(text))
        {
            print("TODO: ERROR UI");
            inputField.text = "";
            return;
        }

        if (currentSlot == firstSlotButton) 
        {
            firstGateCode = firstSlotValidGate[text]; 
            guess[0] = firstGateCode.Value;
        }
        else 
        {
            secondGateCode = secondSlotValidGate[text];
            guess[1] = secondGateCode.Value;
        }

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

        print($"gate values are 1 {guess[0]} and 2 {guess[1]}");

        bool firstResult = FirstGate(firstGateCode.Value, true, true);
        bool finalResult = SecondGate(secondGateCode.Value, firstResult);

        print($"First gate {firstResult}, Final result {finalResult}");
    }

    private bool FirstGate(char gate, bool a, bool b)
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

    private bool SecondGate(char gate, bool a)
    {
        return gate == '6' ? !a : false;
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["ElectricCircuitPuzzleSecondary"];
        cyanCoreImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CyanCoreSecondary"];

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionElectricCircuitPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedElectricCircuitPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}
