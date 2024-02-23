using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionsMenu : MonoBehaviour
{
    // Reference to DialogueData scriptable object
    public DialogueData dialogueData;
    public TMP_Text dialogueText; // Reference to the dialogue text object / sample in the actual menu
    public TMP_Text textSizeDisplay;
    public Slider textSizeSlider;
    public Toggle subtitlesToggle;
    public TMP_Dropdown colorDropdown;

    private const string TextSizeKey = "TextSize";
    private const string SubtitlesKey = "Subtitles";
    private const string ColorIndexKey = "ColorIndex";

    public ScreenShakeScript screenShakeScript;
    public Toggle screenShakeToggle;
    private const string ScreenShakeKey = "ScreenShake";

    // List of colors for the dropdown options
    private readonly Color[] colorOptions = {
        Color.white,
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        new Color(0.5f, 0, 0.5f) // Purple
    };

    public PostProcess postProcess; // Reference to the PostProcess script

    public Toggle bloomToggle;
    public Toggle chromaticAberrationToggle;
    public Toggle vignetteToggle;

    private const string BloomEnabledKey = "BloomEnabled";
    private const string ChromaticAberrationEnabledKey = "ChromaticAberrationEnabled";
    private const string VignetteEnabledKey = "VignetteEnabled";

    private void Start()
    {
        if (postProcess != null)
        {
            // Set initial toggle values based on post-process parameters
            bloomToggle.isOn = PlayerPrefs.GetInt(BloomEnabledKey, 1) == 1;
            chromaticAberrationToggle.isOn = PlayerPrefs.GetInt(ChromaticAberrationEnabledKey, 1) == 1;
            vignetteToggle.isOn = PlayerPrefs.GetInt(VignetteEnabledKey, 1) == 1;

            // Apply initial settings
            ApplyPostProcessSettings();
        }

        // Add listeners for toggle value changes
        bloomToggle.onValueChanged.AddListener(OnBloomToggleChanged);
        chromaticAberrationToggle.onValueChanged.AddListener(OnChromaticAberrationToggleChanged);
        vignetteToggle.onValueChanged.AddListener(OnVignetteToggleChanged);

        if (screenShakeScript != null && screenShakeToggle != null)
        {
            // Load saved preference for screen shake
            if (PlayerPrefs.HasKey(ScreenShakeKey))
            {
                bool screenShakeEnabled = PlayerPrefs.GetInt(ScreenShakeKey) == 1;
                screenShakeToggle.isOn = screenShakeEnabled;
                screenShakeScript.shakeScreen = screenShakeEnabled;
            }

            // Add listener for toggle event
            screenShakeToggle.onValueChanged.AddListener(OnScreenShakeToggleChanged);
        }

        // Populate the color dropdown with color options
        PopulateColorDropdown();

        if (dialogueData != null)
        {
            // Load saved preferences
            if (PlayerPrefs.HasKey(TextSizeKey))
                dialogueData.textSize = PlayerPrefs.GetInt(TextSizeKey);
            if (PlayerPrefs.HasKey(SubtitlesKey))
                dialogueData.subtitlesOn = PlayerPrefs.GetInt(SubtitlesKey) == 1;
            if (PlayerPrefs.HasKey(ColorIndexKey))
            {
                int colorIndex = PlayerPrefs.GetInt(ColorIndexKey);
                colorDropdown.value = colorIndex;
                OnColorDropdownChanged(colorIndex);
            }

            // Update UI
            textSizeSlider.value = dialogueData.textSize;
            subtitlesToggle.isOn = dialogueData.subtitlesOn;
        }

        // Add listeners for UI events
        textSizeSlider.onValueChanged.AddListener(OnTextSizeChanged);
        subtitlesToggle.onValueChanged.AddListener(OnSubtitlesToggleChanged);
        colorDropdown.onValueChanged.AddListener(OnColorDropdownChanged);
    }

    private void OnDestroy()
    {
        // Remove toggle value change listeners to prevent memory leaks
        bloomToggle.onValueChanged.RemoveListener(OnBloomToggleChanged);
        chromaticAberrationToggle.onValueChanged.RemoveListener(OnChromaticAberrationToggleChanged);
        vignetteToggle.onValueChanged.RemoveListener(OnVignetteToggleChanged);

        // Remove listener to prevent memory leaks
        if (screenShakeToggle != null)
        {
            screenShakeToggle.onValueChanged.RemoveListener(OnScreenShakeToggleChanged);
        }
        // Remove listeners to prevent memory leaks
        textSizeSlider.onValueChanged.RemoveListener(OnTextSizeChanged);
        subtitlesToggle.onValueChanged.RemoveListener(OnSubtitlesToggleChanged);
        colorDropdown.onValueChanged.RemoveListener(OnColorDropdownChanged);
    }

    private void PopulateColorDropdown()
    {
        colorDropdown.ClearOptions();

        // Create a list of dropdown options
        var options = new List<TMP_Dropdown.OptionData>();

        // Add each color as a dropdown option
        foreach (Color color in colorOptions)
        {
            var option = new TMP_Dropdown.OptionData(color.ToString());
            options.Add(option);
        }

        // Add options to the dropdown
        colorDropdown.AddOptions(options);
    }

    private void OnScreenShakeToggleChanged(bool value)
    {
        if (screenShakeScript != null)
        {
            // Update screen shake boolean in ScreenShakeScript
            screenShakeScript.shakeScreen = value;

            // Save preference
            PlayerPrefs.SetInt(ScreenShakeKey, value ? 1 : 0);
        }
    }

    private void OnTextSizeChanged(float value)
    {
        dialogueData.textSize = Mathf.RoundToInt(value);
        textSizeDisplay.text = dialogueData.textSize.ToString();

        // Update dialogue text size
        dialogueText.fontSize = dialogueData.textSize;

        PlayerPrefs.SetInt(TextSizeKey, dialogueData.textSize);
    }

    private void OnSubtitlesToggleChanged(bool value)
    {
        dialogueData.subtitlesOn = value;
        PlayerPrefs.SetInt(SubtitlesKey, value ? 1 : 0);
    }

    private void OnColorDropdownChanged(int index)
    {
        // Set the text color based on the selected dropdown option
        dialogueData.textColor = colorOptions[index];
        dialogueText.color = dialogueData.textColor;

        PlayerPrefs.SetInt(ColorIndexKey, index);
    }

    private void OnBloomToggleChanged(bool value)
    {
        PlayerPrefs.SetInt(BloomEnabledKey, value ? 1 : 0);
        ApplyPostProcessSettings();
    }

    private void OnChromaticAberrationToggleChanged(bool value)
    {
        PlayerPrefs.SetInt(ChromaticAberrationEnabledKey, value ? 1 : 0);
        ApplyPostProcessSettings();
    }

    private void OnVignetteToggleChanged(bool value)
    {
        PlayerPrefs.SetInt(VignetteEnabledKey, value ? 1 : 0);
        ApplyPostProcessSettings();
    }

    private void ApplyPostProcessSettings()
    {
        if (postProcess != null)
        {
            // Enable or disable post-processing effects based on toggle values
            postProcess._mBloomIntensity.enabled = bloomToggle.isOn;
            postProcess._mChromaticAberration.enabled = chromaticAberrationToggle.isOn;
            postProcess._mMVignette.enabled = vignetteToggle.isOn;
        }
    }
}
