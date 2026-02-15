using UnityEngine;

public class CutsceneGameplayManager : MonoBehaviour
{
        void Start()
    {
        switch (GameData.escapeRoomNumber)
        {
            case 0:
                GameProgression.GameProgressionInstance.ShowDialogue(Resources.Load<TextAsset>($"Story/Cutscene/cutscene_0"));
                break;
            case 1:
                GameProgression.GameProgressionInstance.ShowDialogue(Resources.Load<TextAsset>($"Story/Cutscene/cutscene_1"));
                break;
            case 2:
                GameProgression.GameProgressionInstance.ShowDialogue(Resources.Load<TextAsset>($"Story/Cutscene/cutscene_2"));
                break;
        }
    }
}
