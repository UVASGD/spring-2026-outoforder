using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AntifreezeDispenserPuzzle : Puzzle
{
    private bool placedDullYellowCore;
    private enum State
    {
        Idle, LetterSelected
    }
    private State currentState = State.Idle;
    private int selectedLetterIndex = -1;
    private GameObject stream;
    private Image yellowCoreImage;
    
    void Awake()
    {
        answer = new char[] { 'A', 'N', 'T', 'I', 'F', 'R', 'E', 'E', 'Z', 'E'};
        guess = new char[] { 'F', 'A', 'R', 'E', 'Z', 'E', 'N', 'I', 'E', 'T'};
        
        // initialize button texts
        for (int i = 0; i < guess.Length; i++)
        {
            TextMeshProUGUI text = transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
            text.text = guess[i].ToString();
        }
        
        stream = transform.Find("Stream").gameObject;
        yellowCoreImage = transform.Find("DullYellowCore").GetComponent<Image>();

        stream.SetActive(false);
    }

    void OnEnable()
    {
        if (GameData.escapeRoomGameplayManager.items["DullYellowCore"].collectible && GameData.escapeRoomGameplayManager.collectedItemsScrollView.Keys.Contains("Dull Yellow Core")) GameData.escapeRoomGameplayManager.items["DullYellowCore"].collectible = false;
    }

    void Update()
    {
        if (!placedDullYellowCore && GameProgression.GameProgressionInstance.GetFlag("usedDullYellowCore"))
        {
            placedDullYellowCore = true;
            yellowCoreImage.enabled = true;
            yellowCoreImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["YellowCore"];
        }
    }

    public void OnLetterClick()
    {
        if (placedDullYellowCore && !solved)
        {
            // if Idle: Highlight letter
            // if LetterSelected: Swap letters
            if (currentState == State.Idle)
            {
                GameObject button = EventSystem.current.currentSelectedGameObject;
                selectedLetterIndex = button.transform.GetSiblingIndex();
                button.GetComponent<Image>().color = Color.yellow;
                currentState = State.LetterSelected;
            }
            else
            {
                GameObject newButton = EventSystem.current.currentSelectedGameObject;
                int newLetterIndex = newButton.transform.GetSiblingIndex();
                
                // swap letters in guess
                (guess[selectedLetterIndex], guess[newLetterIndex]) = (guess[newLetterIndex], guess[selectedLetterIndex]);
                
                
                // update button texts
                TextMeshProUGUI selectedLetterTMP = transform.GetChild(selectedLetterIndex).GetComponentInChildren<TextMeshProUGUI>();
                TextMeshProUGUI newLetterTMP = newButton.GetComponentInChildren<TextMeshProUGUI>();
                selectedLetterTMP.text = guess[selectedLetterIndex].ToString();
                newLetterTMP.text = guess[newLetterIndex].ToString();
                
                // unhighlight previously selected letter
                GameObject previousButton = transform.GetChild(selectedLetterIndex).gameObject;
                previousButton.GetComponent<Image>().color = Color.white;

                currentState = State.Idle;
            }
        }
    }

    public IEnumerator ActivateAntifreezeDispenser()
    {
        Debug.Log("play filling core animation");
        stream.SetActive(true);
        GameProgression.GameProgressionInstance.PlaySFX(9);

        yield return new WaitForSeconds(2);

        stream.SetActive(false);
        yellowCoreImage.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["YellowCoreSecondary"];
    }

    protected override void SolvedPuzzleSpecific()
    {
        yellowCoreImage.gameObject.name = "GlowingYellowCore";
        yellowCoreImage.gameObject.GetComponent<ItemController>().itemData = GameData.escapeRoomGameplayManager.items["GlowingYellowCore"];

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionAntifreezeDispenserPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedAntifreezeDispenserPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}