using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionsMenu : MonoBehaviour
{
    [Header("Dialogue data")]
    // Reference to DialogueData scriptable object
    public DialogueData dialogueData;

    [Header("Dialogue text")]
    public TMP_Text dialogueText; // Reference to the dialogue text object / sample in the actual menu

    [Header("Text size")]
    public TMP_Text textSizeDisplay;
    public Slider textSizeSlider;

    [Header("Subtitle toggle")]
    public Toggle subtitlesToggle;

    [Header("Colour dropdown menu")]
    public TMP_Dropdown colorDropdown;

    [Header("Dyslexia mode toggle")]
    public Toggle dyslexiaToggle; // Add refrence to dyslexia toggle UI element

    private const string TextSizeKey = "TextSize";
    private const string SubtitlesKey = "Subtitles";
    private const string ColorIndexKey = "ColorIndex";
    private const string DyslexiaModeKey = "DyslexiaMode"; // Key for dyslexia
    private const string ScreenShakeKey = "ScreenShake";

    [Header("Screen Shake toggle")]
    public Toggle screenShakeToggle;

    private ScreenShakeScript screenShakeScript;

    private const int DefaultTextSize = 24;
    private const int DefaultTextColorKey = 0;

    private Dictionary<string, Color> colorNameToValue = new Dictionary<string, Color>();

    // List of colors for the dropdown options
    private readonly Color[] colorOptions = {
        Color.white,
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        new Color(0.5f, 0, 0.5f) // Purple
    };

    private PostProcess postProcess; // Reference to the PostProcess script

    [Header("Post processing toggle")]
    public Toggle bloomToggle;
    public Toggle chromaticAberrationToggle;
    public Toggle vignetteToggle;

    private const string BloomEnabledKey = "BloomEnabled";
    private const string ChromaticAberrationEnabledKey = "ChromaticAberrationEnabled";
    private const string VignetteEnabledKey = "VignetteEnabled";

    private void Start()
    {
        // Load saved preference for dyslexia mode
        if (PlayerPrefs.HasKey(DyslexiaModeKey))
        {
            bool dyslexiaModeEnabled = PlayerPrefs.GetInt(DyslexiaModeKey) == 1;
            dyslexiaToggle.isOn = dyslexiaModeEnabled;
            ToggleDyslexiaMode(dyslexiaModeEnabled);
        }

        // Add listener for dyslexia toggle event
        dyslexiaToggle.onValueChanged.AddListener(OnDyslexiaToggleChanged);

        // Ensure that dialogueData is not null before proceeding YOU NEED THIS
        if (dialogueData == null)
        {
            Debug.LogError("DialogueData reference not set in DialogueOptionsMenu.");
            return;
        }

        // Populate the dictionary with color names and values
        InitializeColorDictionary();

        // Populate the color dropdown with color options
        PopulateColorDropdown();

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

    private void OnDyslexiaToggleChanged(bool value)
    {
        ToggleDyslexiaMode(value);
        PlayerPrefs.SetInt(DyslexiaModeKey, value ? 1 : 0);
    }

    private void ToggleDyslexiaMode(bool isEnabled)
    {
        // Only apply dyslexic font if dyslexia mode is explicitly enabled by the user
        if (isEnabled)
        {
            // Set dyslexia mode in dialogue data
            dialogueData.useDyslexicFont = true;

            // Apply dyslexic font
            dialogueText.font = dialogueData.dyslexicFont;
        }
        else
        {
            // If dyslexia mode is disabled, use the default font
            dialogueData.useDyslexicFont = false;
            dialogueText.font = dialogueData.defaultFont; 
        }
    }

    private void InitializeColorDictionary()
    {
        colorNameToValue.Clear();
        colorNameToValue.Add("White", Color.white);
        colorNameToValue.Add("Red", Color.red);
        colorNameToValue.Add("Green", Color.green);
        colorNameToValue.Add("Blue", Color.blue);
        colorNameToValue.Add("Yellow", Color.yellow);
        colorNameToValue.Add("Purple", new Color(0.5f, 0, 0.5f));
    }

    private void PopulateColorDropdown()
    {
        colorDropdown.ClearOptions();

        // Create a list of dropdown options
        var options = new List<TMP_Dropdown.OptionData>();

        // Add each color name as a dropdown option
        foreach (string colorName in colorNameToValue.Keys)
        {
            var option = new TMP_Dropdown.OptionData(colorName);
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

    private void OnEnable()
    {
        dialogueText.fontSize = PlayerPrefs.GetInt(TextSizeKey, DefaultTextSize);
        textSizeSlider.value = PlayerPrefs.GetInt(TextSizeKey, DefaultTextSize);
        dialogueText.color = colorOptions[PlayerPrefs.GetInt(ColorIndexKey, DefaultTextColorKey)];
    }
}
