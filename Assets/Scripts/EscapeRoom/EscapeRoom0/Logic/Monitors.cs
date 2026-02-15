using UnityEngine;
using UnityEngine.UI;

public class Monitors : MonoBehaviour
{
    private bool monitorsOn;

    void Update()
    {
        if (GameData.escapeRoomGameplayManagerScript.interactingWith.Equals("Monitors") && !monitorsOn && GameProgression.GameProgressionInstance.GetFlag("firstInteractionRemote") && GameProgression.GameProgressionInstance.GetFlag("firstInteractionMonitors"))
        {
            monitorsOn = true;
            GameData.escapeRoomGameplayManagerScript.locations[4].GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["Location4Secondary"];
        }
    }
}
