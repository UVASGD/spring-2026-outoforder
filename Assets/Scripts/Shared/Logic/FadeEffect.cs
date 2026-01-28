using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class FadeEffect : MonoBehaviour
{
    public void FadeIn(GameObject gameObjectToFadeIn, float fadeTime = 0.1f, string scene="", float fadeDelay = 0f)
    {
        if (GameData.fadeCoroutine == null) GameData.fadeCoroutine = StartCoroutine(Fade(gameObjectToFadeIn, 0, 1, fadeTime: fadeTime, scene: scene, fadeDelay: fadeDelay));
    }

    public void FadeOut(GameObject gameObjectToFadeOut, float fadeTime = 0.1f, bool transitioning = false)
    {
        if (GameData.fadeCoroutine == null) StartCoroutine(Fade(gameObjectToFadeOut, 1, 0, fadeTime: fadeTime, transitioning: transitioning));
    }

    private IEnumerator Fade(GameObject gameObjectToFade, float startA, float endA, float fadeTime = 0.1f, bool transitioning = false, string scene = "", float fadeDelay = 0f)
    {
        if (fadeDelay != 0)
        {
            yield return new WaitForSeconds(fadeDelay);
        }

        if (startA == 0)
        {
            gameObjectToFade.SetActive(true);
        }

        float time = 0f;

        while (time < fadeTime) // default to 0.1 but can specify otherwise (to 1 for scene transitions)
        {
            if (gameObjectToFade == null || gameObjectToFade.GetComponent<CanvasGroup>() == null)
            {
                GameData.fadeCoroutine = null;
                Debug.LogWarning($"Fade target {gameObjectToFade.name} was destroyed mid-fade. Exiting coroutine.");
                yield break;
            }
            time += Time.deltaTime;
            float a = Mathf.Lerp(startA, endA, time / fadeTime);
            gameObjectToFade.GetComponent<CanvasGroup>().alpha = a;
            yield return null;
        }

        gameObjectToFade.GetComponent<CanvasGroup>().alpha = endA;

        if (endA == 0)
        {
            if (gameObjectToFade.GetComponent<Image>() != null && !gameObjectToFade.name.Contains("DialogueSystemManager") && !gameObjectToFade.name.Contains("BlackTransition") && !gameObjectToFade.name.Contains("CaseStatus"))
            {
                gameObjectToFade.GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["Transparent"];
            }
            gameObjectToFade.SetActive(false);
        }

        if (!scene.Equals(""))
        {
            SceneManager.LoadScene(scene);
        }

        if (transitioning) GameProgression.GameProgressionInstance.transitioning = false;

        GameData.fadeCoroutine = null;
    }
}
