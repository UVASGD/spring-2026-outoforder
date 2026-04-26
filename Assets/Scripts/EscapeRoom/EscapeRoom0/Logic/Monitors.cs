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
            GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["MonitorsSecondary"];
            GameData.escapeRoomGameplayManager.locationGameObjects[4].GetComponent<Image>().sprite = GameProgression.GameProgressionInstance.SpriteCache.sprites["EscapeRoom0Location4Secondary"];
        }
    }
}
