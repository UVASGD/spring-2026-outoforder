using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockedBoxPuzzle : Puzzle
{
    private GameObject replacementRedCore;
    private Dictionary<Color, char> colorToIndex = new Dictionary<Color, char>
    {
        { Color.cyan, '0' },
        { Color.magenta, '1' },
        { Color.yellow, '2' }
    };
    private List<Color> colorPalette = new List<Color> { Color.cyan, Color.magenta, Color.yellow };

    void Awake()
    {
        answer = new char[] { '0', '1', '2', '1', '0' };
        guess = new char[] { '1', '1', '1', '1', '1' };

        replacementRedCore = transform.Find("ReplacementRedCore").gameObject;

        GetComponentsInChildren<Image>()
            .Where(color => color.name.Contains("Color"))
            .ToList()
            .ForEach(color => color.alphaHitTestMinimumThreshold = 0.1f);

        replacementRedCore.SetActive(false);
    }

    public void IncrementColor() {
        GameObject color = EventSystem.current.currentSelectedGameObject;
        Image colorImage = color.GetComponentInChildren<Image>();
        
        int colorIndex = colorToIndex[colorImage.color];
        
        int nextColorIndex = (colorIndex + 1) % colorPalette.Count;
        print($"next color inded is {nextColorIndex}");
        colorImage.color = colorPalette[nextColorIndex];
        
        guess[color.transform.GetSiblingIndex()] = colorToIndex[colorImage.color];
    }

    protected override void SolvedPuzzleSpecific()
    {
        gameObject.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["LockedBoxPuzzleSecondary"];
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        replacementRedCore.SetActive(true);

        GameProgression.GameProgressionInstance.SetFlag("firstLockedBoxPuzzle", true);
        GameProgression.GameProgressionInstance.SetFlag("solvedLockedBoxPuzzle", true);
        gameObject.GetComponent<ManualInteraction>().ItemInteraction();
    }
}
