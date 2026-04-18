using UnityEngine;

public class VisualNovelGameplay : MonoBehaviour
{
    private TextAsset visualNovelJSONFile;

    void Start()
    {
        visualNovelJSONFile = Resources.Load<TextAsset>($"Story/VisualNovel/visual_novel_{GameData.escapeRoomNumber}");
        GameProgression.GameProgressionInstance.ShowDialogue(visualNovelJSONFile);
    }
}
