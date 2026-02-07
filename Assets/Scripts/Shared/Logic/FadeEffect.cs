using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void FadeInCGSprite(GameObject gameObject, Sprite spriteToFadeIn)
    {
        if (GameData.fadeCoroutine == null) GameData.fadeCoroutine = StartCoroutine(FadeCGSprite(gameObject, spriteToFadeIn, 0, 1));
    }

    public void FadeOutCGSprite(GameObject gameObject, Sprite spriteToFadeOut)
    {
        if (GameData.fadeCoroutine == null) StartCoroutine(FadeCGSprite(gameObject, spriteToFadeOut, 1, -1));
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

     // if sprite is null, indicates that it is an imageless graphic i.e. text only
    private IEnumerator FadeCGSprite(GameObject currentCharacterDisplay, Sprite sprite, double fadeFrom, double fadeTo, float speed = 0.1f, bool isUIElement = false, bool getTextFromChild = false) 
    {
        double desiredOpacity = fadeTo;

        // image component
        Image characterDisplayImage = currentCharacterDisplay.GetComponent<Image>();
        Color imageColor = Color.red;
        if (characterDisplayImage != null) 
        {
            imageColor = characterDisplayImage.color;
            characterDisplayImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, imageColor.a);
            characterDisplayImage.sprite = sprite;
        }
        
        // text component
        TextMeshProUGUI characterDisplayText = currentCharacterDisplay.GetComponent<TextMeshProUGUI>();
        if (getTextFromChild) 
        {
            characterDisplayText = currentCharacterDisplay.GetComponentInChildren<TextMeshProUGUI>();
        }
        Color textColor = Color.red;
        if (characterDisplayText != null)
        {
            textColor = characterDisplayText.color;
        }

        Func<double, double, bool> lte = (a, b) => a <= b;
        Func<double, double, bool> gte = (a, b) => b <= a;

        double increment = speed;
        double until = desiredOpacity + increment;
        Func<double, double, bool> comparisonToUse = lte;

        // fading out (fading in = default)
        if (fadeFrom > fadeTo) {
            increment = -0.1f;
            until = desiredOpacity;
            comparisonToUse = gte;
        }

        // as UI elements can have varied opacity
        if (isUIElement) 
        {
            fadeFrom = imageColor.a;
        }

        for (double i = fadeFrom; comparisonToUse(i, until); i += increment) 
        {
            if (characterDisplayImage != null)
            {
                if ((isUIElement && i < increment / 2)|| !isUIElement)
                {
                    characterDisplayImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, (float) i);
                }
            }
            if (characterDisplayText != null) 
            {
                characterDisplayText.color = new Color(textColor.r, textColor.g, textColor.b, (float) i);
            }
            yield return null;
        }

        GameData.fadeCoroutine = null;
    }
}
