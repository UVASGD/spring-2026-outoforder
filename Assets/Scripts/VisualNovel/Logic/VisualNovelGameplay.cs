using System.Collections.Generic;
using UnityEngine;

public class VisualNovelGameplay : MonoBehaviour
{
    private TextAsset visualNovelJSONFile;

    void Awake()
    {
        visualNovelJSONFile = Resources.Load<TextAsset>($"Story/VisualNovel/visual_novel_{GameData.visualNovelNumber}");
    }

    void Start()
    {
        GameProgression.GameProgressionInstance.ShowDialogue(visualNovelJSONFile);
    }
}
