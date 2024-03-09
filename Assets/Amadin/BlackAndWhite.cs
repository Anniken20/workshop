using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class BrightnessControl : MonoBehaviour
{
    public Slider brightnessSlider; // Reference to the slider UI element
    public float minBrightness = -20f; // Minimum brightness value
    public float maxBrightness = 20f; // Maximum brightness value

    private PostProcessVolume volume; // Reference to the Post-Processing Volume
    private PostProcessProfile profile; // Reference to the Post-Processing Profile
    private ColorGrading colorGrading; // Reference to the Color Grading effect
    private const string brightnessKey = "BrightnessSetting"; // Key for storing brightness in PlayerPrefs

    void Start()
    {
        // Get the Post-Processing Volume component attached to the camera
        volume = GetComponent<PostProcessVolume>();

        // If a Post-Processing Volume component is not found, create one
        if (volume == null)
        {
            volume = gameObject.AddComponent<PostProcessVolume>();
            volume.isGlobal = true;
        }

        // Assign the existing Post-Processing Profile named "Brightness" or create a new one
        profile = volume.sharedProfile ? volume.sharedProfile : ScriptableObject.CreateInstance<PostProcessProfile>();
        volume.sharedProfile = profile;

        // Get or set the brightness setting from PlayerPrefs
        float brightnessValue = PlayerPrefs.GetFloat(brightnessKey, 0f);
        SetBrightness(brightnessValue);

        // Add listener for value changes in the slider
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    void SetBrightness(float brightnessValue)
    {
        // Ensure brightness value is within range
        brightnessValue = Mathf.Clamp(brightnessValue, minBrightness, maxBrightness);

        // Save the brightness setting to PlayerPrefs
        PlayerPrefs.SetFloat(brightnessKey, brightnessValue);
        PlayerPrefs.Save();

        // Modify the post exposure setting in the Post-Processing Profile
        var colorGradingSettings = profile.GetSetting<ColorGrading>();
        if (colorGradingSettings != null)
        {
            // Adjust the post exposure value
            colorGradingSettings.postExposure.value = brightnessValue;
        }
        else
        {
            // If ColorGrading settings not found, add it to the profile and set brightness
            colorGradingSettings = profile.AddSettings<ColorGrading>();
            colorGradingSettings.postExposure.value = brightnessValue;
        }
    }

}
