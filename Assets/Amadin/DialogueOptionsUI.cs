using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionsMenu : MonoBehaviour
{
    [Header("Dialogue data")]
    public DialogueData dialogueData;

    [Header("Dialogue text")]
    public TMP_Text dialogueText;

    [Header("Text size")]
    public TMP_Text textSizeDisplay;
    public Slider textSizeSlider;

    [Header("Subtitle toggle")]
    public Toggle subtitlesToggle;

    [Header("Colour dropdown menu")]
    public TMP_Dropdown colorDropdown;

    [Header("Dyslexia mode toggle")]
    public Toggle dyslexiaToggle;

    private const string TextSizeKey = "TextSize";
    private const string SubtitlesKey = "Subtitles";
    private const string ColorIndexKey = "ColorIndex";
    private const string DyslexiaModeKey = "DyslexiaMode";

    private const int DefaultTextSize = 24;
    private const int DefaultTextColorIndex = 0;

    private Dictionary<string, Color> colorNameToValue = new Dictionary<string, Color>();
    private readonly Color[] colorOptions = {
        Color.white,
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        new Color(0.5f, 0, 0.5f) // Purple
    };

    private void Start()
    {
        if (dialogueData == null)
        {
            Debug.LogError("DialogueData reference not set in DialogueOptionsMenu.");
            return;
        }

        InitializeColorDictionary();
        PopulateColorDropdown();
        LoadPlayerPrefs();

        // Set default values
        dialogueData.textSize = DefaultTextSize;
        dialogueText.fontSize = DefaultTextSize;
        dialogueData.textColor = colorOptions[DefaultTextColorIndex];
        dialogueText.color = dialogueData.textColor;

        // Enable all options except dyslexia mode
        subtitlesToggle.isOn = true;
        colorDropdown.value = DefaultTextColorIndex;
        dyslexiaToggle.isOn = false; // Dyslexia mode is off by default

        textSizeSlider.value = DefaultTextSize;
        textSizeDisplay.text = DefaultTextSize.ToString();

        // Add listeners
        textSizeSlider.onValueChanged.AddListener(OnTextSizeChanged);
        subtitlesToggle.onValueChanged.AddListener(OnSubtitlesToggleChanged);
        colorDropdown.onValueChanged.AddListener(OnColorDropdownChanged);
        dyslexiaToggle.onValueChanged.AddListener(OnDyslexiaToggleChanged);
    }

    private void OnDestroy()
    {
        textSizeSlider.onValueChanged.RemoveListener(OnTextSizeChanged);
        subtitlesToggle.onValueChanged.RemoveListener(OnSubtitlesToggleChanged);
        colorDropdown.onValueChanged.RemoveListener(OnColorDropdownChanged);
        dyslexiaToggle.onValueChanged.RemoveListener(OnDyslexiaToggleChanged);
    }

    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(TextSizeKey))
        {
            dialogueData.textSize = PlayerPrefs.GetInt(TextSizeKey);
            textSizeSlider.value = dialogueData.textSize;
            textSizeDisplay.text = dialogueData.textSize.ToString();
        }
        else
        {
            textSizeSlider.value = DefaultTextSize;
            textSizeDisplay.text = DefaultTextSize.ToString();
        }

        if (PlayerPrefs.HasKey(SubtitlesKey))
        {
            dialogueData.subtitlesOn = PlayerPrefs.GetInt(SubtitlesKey) == 1;
            subtitlesToggle.isOn = dialogueData.subtitlesOn;
        }

        if (PlayerPrefs.HasKey(ColorIndexKey))
        {
            int colorIndex = PlayerPrefs.GetInt(ColorIndexKey);
            colorDropdown.value = colorIndex;
            OnColorDropdownChanged(colorIndex);
        }

        if (PlayerPrefs.HasKey(DyslexiaModeKey))
        {
            bool dyslexiaModeEnabled = PlayerPrefs.GetInt(DyslexiaModeKey) == 1;
            dyslexiaToggle.isOn = dyslexiaModeEnabled;
            ToggleDyslexiaMode(dyslexiaModeEnabled);
        }
    }

    public void ToggleDyslexiaMode(bool isEnabled)
    {
        dialogueData.useDyslexicFont = isEnabled;
        PlayerPrefs.SetInt(DyslexiaModeKey, isEnabled ? 1 : 0);

        TMP_FontAsset font = isEnabled ? dialogueData.dyslexicFont : dialogueData.defaultFont;
        ApplyFontToAllText(font);
    }

    private void ApplyFontToAllText(TMP_FontAsset font)
    {
        TMP_Text[] textComponents = FindObjectsOfType<TMP_Text>(true);
        foreach (TMP_Text textComponent in textComponents)
        {
            textComponent.font = font;
        }
    }

    private void OnDyslexiaToggleChanged(bool value)
    {
        ToggleDyslexiaMode(value);
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
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (string colorName in colorNameToValue.Keys)
        {
            var option = new TMP_Dropdown.OptionData(colorName);
            options.Add(option);
        }

        colorDropdown.AddOptions(options);
    }

    private void OnTextSizeChanged(float value)
    {
        int newSize = Mathf.RoundToInt(value);
        dialogueData.textSize = newSize;
        dialogueText.fontSize = newSize;
        textSizeDisplay.text = newSize.ToString();
        PlayerPrefs.SetInt(TextSizeKey, newSize);
    }

    private void OnSubtitlesToggleChanged(bool value)
    {
        dialogueData.subtitlesOn = value;
        PlayerPrefs.SetInt(SubtitlesKey, value ? 1 : 0);
    }

    private void OnColorDropdownChanged(int index)
    {
        dialogueData.textColor = colorOptions[index];
        dialogueText.color = dialogueData.textColor;
        PlayerPrefs.SetInt(ColorIndexKey, index);
    }
}
