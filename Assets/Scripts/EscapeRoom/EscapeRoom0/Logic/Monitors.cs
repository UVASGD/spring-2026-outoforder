using UnityEngine;
using UnityEngine.UI;

public class Monitors : MonoBehaviour
{
    private bool monitorsOn;

    void Update()
    {
        if (!monitorsOn && GameProgression.GameProgressionInstance.GetFlag("usedRemote"))
        {
            monitorsOn = true;
            GameData.escapeRoomGameplayManagerScript.locations[4].GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["Location4Secondary"];
        }
    }
}
