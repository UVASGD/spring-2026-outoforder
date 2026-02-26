using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaesarDiaryPuzzle : Puzzle
{
    TextMeshProUGUI[] letters;
    char[] resetValue = new char[] { 'C', 'A', 'E', 'S', 'A', 'R' };
    private GameObject flashDrive;
    
    void Awake()
    {
        answer = new char[] { 'K', 'I', 'M', 'A', 'I', 'Z' };
        guess = new char[] { 'C', 'A', 'E', 'S', 'A', 'R' };

        letters = transform.GetComponentsInChildren<TextMeshProUGUI>();

        flashDrive = transform.parent.Find("Interactables/FlashDrive").gameObject;
        flashDrive.SetActive(false);
        print($"flash drive is now {flashDrive.activeSelf}");
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["CaesarDiaryPuzzleSecondary"];

        // TODO: this stays active if you exit out before collecting it
        flashDrive.SetActive(true);

        GameProgression.GameProgressionInstance.SetFlag("firstInteractionCaesarDiaryPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedCaesarDiaryPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }

    public void ResetPuzzle()
    {
        for (int i = 0; i < letters.Length; i++)
        {
            letters[i].text = resetValue[i].ToString();
        }

        guess = resetValue;
    }
}
