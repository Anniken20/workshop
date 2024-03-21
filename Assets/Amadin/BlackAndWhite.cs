using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PostProcessingController : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public Slider gammaSlider;
    public Slider brightnessSlider;
    public Slider contrastSlider;

    private ColorGrading colorGrading;

    private bool slidersChanged = false; // Flag to track if sliders have been changed

    void Start()
    {
        if (postProcessVolume == null)
        {
            Debug.LogError("Post Process Volume not assigned!");
            return;
        }

        postProcessVolume.profile.TryGetSettings(out colorGrading);

        if (colorGrading == null)
        {
            Debug.LogError("Color Grading not found in Post Process Volume!");
            return;
        }

        // Load saved settings or defaults if sliders have been changed
        if (PlayerPrefs.HasKey("SlidersChanged"))
        {
            gammaSlider.value = PlayerPrefs.GetFloat("Gamma", 0f);
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 0f);
            contrastSlider.value = PlayerPrefs.GetFloat("Contrast", 0f);
        }

        ApplySettings();
    }

    public void OnGammaChange(float value)
    {
        PlayerPrefs.SetFloat("Gamma", value);
        slidersChanged = true;
        ApplySettings();
    }

    public void OnBrightnessChange(float value)
    {
        PlayerPrefs.SetFloat("Brightness", value);
        slidersChanged = true;
        ApplySettings();
    }

    public void OnContrastChange(float value)
    {
        PlayerPrefs.SetFloat("Contrast", value);
        slidersChanged = true;
        ApplySettings();
    }

    private void ApplySettings()
    {
        if (!slidersChanged) // Only apply settings if sliders have been changed
            return;

        float gamma = Mathf.Pow(2f, gammaSlider.value - 10f); // Scale and offset slider value
        float brightness = Mathf.Clamp(brightnessSlider.value - 10f, -1f, 1f); // Scale and offset slider value to range [-1, 1]
        float contrast = Mathf.Clamp(contrastSlider.value - 10f, -1f, 1f); // Scale and offset slider value to range [-1, 1]

        colorGrading.gamma.value = new Vector4(gamma, gamma, gamma, 1f);
        colorGrading.postExposure.value = brightness;
        colorGrading.contrast.value = contrast;

        PlayerPrefs.SetInt("SlidersChanged", 1); // Save flag indicating sliders have been changed
    }
}
