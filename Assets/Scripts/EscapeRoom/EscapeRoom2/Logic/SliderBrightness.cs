using UnityEngine;
using UnityEngine.UI;

public class SliderBrightness : MonoBehaviour
{
    public Image mask;
    public Slider slider;
    void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck();});

        mask = GetComponent<Image>();
        var tempColor = mask.color;
        tempColor.a = 1f;
        mask.color = tempColor;
        SetImageAlpha(1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValueChangeCheck()
    {
        Debug.Log("Slider value is: " + slider.value);
        // SetImageAlpha(slider.value); // Change Image opacity
        // valueText.text = mainSlider.value.ToString(); // Update text here if linked
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
