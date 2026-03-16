using UnityEngine;

public class MirrorZoom : MonoBehaviour
{
    private bool postInteraction;

    void Update()
    {
        if (!postInteraction && GameProgression.GameProgressionInstance.GetFlag("firstInteractionMirrorZoom"))
        {
            postInteraction = true;
            gameObject.name = "MirrorZoomPostInteraction";
        }
    }
}
