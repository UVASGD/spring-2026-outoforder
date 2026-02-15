using TMPro;
using UnityEngine.UI;

public class CaesarDiaryPuzzle : Puzzle
{
    TextMeshProUGUI[] letters;
    char[] resetValue = new char[] { 'C', 'A', 'E', 'S', 'A', 'R' };
    
    void Awake()
    {
        answer = new char[] { 'K', 'I', 'M', 'A', 'I', 'Z' };
        guess = new char[] { 'C', 'A', 'E', 'S', 'A', 'R' };

        letters = transform.GetComponentsInChildren<TextMeshProUGUI>();
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["UnlockedCaesarDiaryPuzzle"];

        GameProgression.GameProgressionInstance.SetFlag("solvedCaesarDiaryPuzzle", true);
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
