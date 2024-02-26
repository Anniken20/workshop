using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSettingsController : MonoBehaviour
{
    public TMP_Text dialogueText;
    public DialogueData dialogueData;
    public Image colorDisplayImage;
    public GameObject colorPicker;
    public Color[] colorOptions; // Array of colors to choose from

    private void Start()
    {
        // Load color from PlayerPrefs
        if (PlayerPrefs.HasKey("TextColor"))
        {
            string savedColorString = PlayerPrefs.GetString("TextColor");
            Color savedColor = ColorUtility.TryParseHtmlString(savedColorString, out Color color) ? color : Color.white;
            SetTextColor(savedColor);
        }
    }

    // Method to set text size
    public void SetTextSize(float size)
    {
        dialogueText.fontSize = Mathf.RoundToInt(size);
        PlayerPrefs.SetInt("TextSize", Mathf.RoundToInt(size));
    }

    // Method to set text color
    public void SetTextColor(Color color)
    {
        dialogueText.color = color;
        colorDisplayImage.color = color;
        dialogueData.textColor = color;

        // Save color to PlayerPrefs
        string colorString = ColorUtility.ToHtmlStringRGB(color);
        PlayerPrefs.SetString("TextColor", colorString);
    }

    // Method to open color picker
    public void OpenColorPicker()
    {
        colorPicker.SetActive(true);
    }

    // Method to toggle subtitles
    public void ToggleSubtitles(bool value)
    {
        dialogueData.subtitlesOn = value;
    }
}
