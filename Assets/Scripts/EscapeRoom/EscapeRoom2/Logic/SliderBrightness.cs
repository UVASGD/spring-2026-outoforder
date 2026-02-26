using UnityEngine;
using UnityEngine.UI;

public class SliderBrightness : MonoBehaviour
{
    public Image mask;
    public Slider slider;
    void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck();});
        var tempColor = mask.color;
        tempColor.a = 1f;
        mask.color = tempColor;
        SetImageAlpha(0f); // Start transparent
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValueChangeCheck()
    {
        SetImageAlpha(slider.value);
    }

    public void SetImageAlpha(float alphaValue)
    {
        if (mask != null)
        {
            Color tempColor = mask.color; // Get the current color struct
            tempColor.a = alphaValue; // Modify the alpha component
            mask.color = tempColor; // Reassign the modified color back to the image
        }
    }
}
