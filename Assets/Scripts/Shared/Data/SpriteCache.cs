using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteCache", menuName = "ScriptableObjects/SpriteCache")]
public class SpriteCache : ScriptableObject
{
    public Dictionary<string, Sprite> sprites = new();
    public Dictionary<string, AnimationClip> animationClips = new();

    void OnEnable()
    {
        LoadSpritesFromPath("Art/VisualNovel/");
    }

    void LoadSpritesFromPath(string path)
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(path);

        foreach (var loadedSprite in loadedSprites)
        {
            sprites[loadedSprite.name] = loadedSprite;

            // Debug.Log($"loaded {loadedSprite.name}");
        }
    }
}
