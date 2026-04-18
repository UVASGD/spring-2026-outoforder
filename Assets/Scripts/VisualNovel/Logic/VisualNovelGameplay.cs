using UnityEngine;

public class VisualNovelGameplay : MonoBehaviour
{
    private TextAsset visualNovelJSONFile;

    void Start()
    {
        if (GameData.loadedSave != -1) GameProgression.GameProgressionInstance.DialogueSystem.dialogueIndex = PlayerPrefs.GetInt($"{GameData.loadedSave}_dialogueIndex");

        visualNovelJSONFile = Resources.Load<TextAsset>($"Story/VisualNovel/visual_novel_{GameData.escapeRoomNumber}");
        GameProgression.GameProgressionInstance.ShowDialogue(visualNovelJSONFile);

        if (GameData.loadedSave != -1)
        {
            GameProgression.GameProgressionInstance.DialogueSystem.locationTMP.text = PlayerPrefs.GetString($"{GameData.loadedSave}_locationTMP");

            GameProgression.GameProgressionInstance.DialogueSystem.speakerSpriteAstaImageActive.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[PlayerPrefs.GetString($"{GameData.loadedSave}_speakerSpriteAstaImageActive")];
            GameProgression.GameProgressionInstance.DialogueSystem.speakerSpriteAstaImageOld.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[PlayerPrefs.GetString($"{GameData.loadedSave}_speakerSpriteAstaImageOld")];
            GameProgression.GameProgressionInstance.DialogueSystem.speakerSpriteVirgoImageActive.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[PlayerPrefs.GetString($"{GameData.loadedSave}_speakerSpriteVirgoImageActive")];
            GameProgression.GameProgressionInstance.DialogueSystem.speakerSpriteVirgoImageOld.sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites[PlayerPrefs.GetString($"{GameData.loadedSave}_speakerSpriteVirgoImageOld")];

            StartCoroutine(GameProgression.GameProgressionInstance.PlayBGM(PlayerPrefs.GetInt($"{GameData.loadedSave}_currentBGM")));

            GameData.loadedSave = -1;
        }
    }
}
