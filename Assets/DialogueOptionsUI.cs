using TMPro; 
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionsMenu : MonoBehaviour
{
    // Reference to DialogueData scriptable object
    public DialogueData dialogueData; 
    public TMP_Text textSizeDisplay;
    public Slider textSizeSlider;
    public Toggle subtitlesToggle;
    public Dropdown colorDropdown;

    private const string TextSizeKey = "TextSize";
    private const string SubtitlesKey = "Subtitles";
    private const string ColorIndexKey = "ColorIndex";

    private void Start()
    {
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
        // Remove listeners to prevent memory leaks
        textSizeSlider.onValueChanged.RemoveListener(OnTextSizeChanged);
        subtitlesToggle.onValueChanged.RemoveListener(OnSubtitlesToggleChanged);
        colorDropdown.onValueChanged.RemoveListener(OnColorDropdownChanged);
    }

    private void OnTextSizeChanged(float value)
    {
        dialogueData.textSize = Mathf.RoundToInt(value);
        textSizeDisplay.text = dialogueData.textSize.ToString();
        PlayerPrefs.SetInt(TextSizeKey, dialogueData.textSize);
    }

    private void OnSubtitlesToggleChanged(bool value)
    {
        dialogueData.subtitlesOn = value;
        PlayerPrefs.SetInt(SubtitlesKey, value ? 1 : 0);
    }

    private void OnColorDropdownChanged(int index)
    {
        switch (index)
        {
            case 0: // White
                dialogueData.textColor = Color.white;
                break;
            case 1: // Red
                dialogueData.textColor = Color.red;
                break;
            case 2: // Green
                dialogueData.textColor = Color.green;
                break;
            case 3: // Blue
                dialogueData.textColor = Color.blue;
                break;
            case 4: // Yellow
                dialogueData.textColor = Color.yellow;
                break;
            case 5: // Purple
                dialogueData.textColor = new Color(0.5f, 0, 0.5f);
                break;
        }
        PlayerPrefs.SetInt(ColorIndexKey, index);
    }
}