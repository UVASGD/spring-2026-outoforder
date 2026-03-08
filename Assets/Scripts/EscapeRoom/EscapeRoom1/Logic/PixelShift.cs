// using UnityEngine;
// using UnityEngine.UI;

// public class PixelShift : MonoBehaviour
// {
//     private bool folderVisible;

//     void Awake()
//     {
//         GetComponent<Image>().enabled = false;
//         GetComponent<Button>().enabled = false;
//     }

//     void Update()
//     {
//         if (!folderVisible && GameProgression.GameProgressionInstance.GetFlag("usedFlashDrive"))
//         {
//             folderVisible = true;
//             GetComponent<Image>().enabled = true;
//             GetComponent<Button>().enabled = true;
//         }
//     }
// }
