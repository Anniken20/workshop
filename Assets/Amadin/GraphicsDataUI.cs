using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GraphicsDataUI : MonoBehaviour
{
    public GraphicsData graphicsData;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Button resetButton; // Add reset button

    private void Start()
    {
        PopulateResolutionDropdown();
        PopulateQualityDropdown();
        LoadSettings();
        resetButton.onClick.AddListener(ResetSettings); // Add listener to reset button
    }

    // Populate resolution dropdown with available resolutions
    private void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        // Add available resolutions
        AddResolutionOption(2560, 1440, "16:9");
        AddResolutionOption(1920, 1080, "16:9");
        AddResolutionOption(1366, 768, "16:9");
        AddResolutionOption(1280, 720, "16:9");

        AddResolutionOption(1920, 1200, "16:10");
        AddResolutionOption(1680, 1050, "16:10");
        AddResolutionOption(1440, 900, "16:10");
        AddResolutionOption(1280, 800, "16:10");

        AddResolutionOption(1024, 768, "4:3");
        AddResolutionOption(800, 600, "4:3");
        AddResolutionOption(640, 480, "4:3");
    }

    // Add a resolution option to the dropdown
    private void AddResolutionOption(int width, int height, string aspectRatio)
    {
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(width + "x" + height + " (" + aspectRatio + ")"));
    }

    // Populate quality dropdown with options
    private void PopulateQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string> { "Low", "Medium", "High", "Ultra" };
        qualityDropdown.AddOptions(qualityOptions);

        // Set default quality level based on system capability
        int defaultQualityLevel = GetDefaultQualityLevel();
        qualityDropdown.value = defaultQualityLevel;
        graphicsData.qualityLevel = defaultQualityLevel;
    }

    // Get default quality level based on system capability
    private int GetDefaultQualityLevel()
    {
        int qualityLevel = 1; // Default to medium
        if (SystemInfo.graphicsMemorySize < 1024) // Low-end system
            qualityLevel = 0; // Low
        else if (SystemInfo.graphicsMemorySize > 2048) // High-end system
            qualityLevel = 3; // Ultra
        return qualityLevel;
    }

    // Load graphics settings from PlayerPrefs
    private void LoadSettings()
    {
        // Load resolution
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        resolutionDropdown.value = resolutionIndex;
        OnResolutionDropdownChanged(resolutionIndex); // Update resolution

        // Load quality level
        int qualityLevel = PlayerPrefs.GetInt("QualityLevel", GetDefaultQualityLevel());
        qualityDropdown.value = qualityLevel;
        graphicsData.qualityLevel = qualityLevel;

        // Load fullscreen
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1 ? true : false;
        fullscreenToggle.isOn = fullscreen;
        OnFullscreenToggleChanged(fullscreen); // Update fullscreen

        // Set default values visually
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.RefreshShownValue();

        // Ensure fullscreen is always on
        fullscreenToggle.isOn = true;

        // Recalculate and set default values based on system capability
        qualityLevel = GetDefaultQualityLevel();
        qualityDropdown.value = qualityLevel;
        graphicsData.qualityLevel = qualityLevel;

        Resolution currentResolution = Screen.currentResolution;
        string currentResolutionString = currentResolution.width + "x" + currentResolution.height;
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if (resolutionDropdown.options[i].text == currentResolutionString)
            {
                resolutionDropdown.value = i;
                break;
            }
        }
    }

    // Save graphics settings to PlayerPrefs
    private void SaveSettings()
    {
        // Save resolution
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);

        // Save quality level
        PlayerPrefs.SetInt("QualityLevel", qualityDropdown.value);

        // Save fullscreen
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);

        // Ensure PlayerPrefs changes are saved
        PlayerPrefs.Save();
    }

    // Called when resolution dropdown changes
    public void OnResolutionDropdownChanged(int index)
    {
        // Update resolution settings
        Resolution selectedResolution = GetResolutionFromIndex(index);
        SetResolution(selectedResolution);

        SaveSettings(); // Save settings after update
    }

    // Called when quality dropdown changes
    public void OnQualityDropdownChanged(int index)
    {
        graphicsData.qualityLevel = index;
        SaveSettings(); // Save settings after update
    }

    // Called when fullscreen toggle changes
    public void OnFullscreenToggleChanged(bool newValue)
    {
        SetFullscreen(newValue); // Update fullscreen settings
        SaveSettings(); // Save settings after update
    }

    // Set fullscreen mode
    private void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    // Method to get resolution from dropdown index
    private Resolution GetResolutionFromIndex(int index)
    {
        string selectedResolutionString = resolutionDropdown.options[index].text;
        string[] resolutionParts = selectedResolutionString.Split('x');
        int width = int.Parse(resolutionParts[0]);
        int height = int.Parse(resolutionParts[1].Split(' ')[0]);
        Resolution resolution = new Resolution
        {
            width = width,
            height = height
        };
        return resolution;
    }

    // Method to set resolution
    private void SetResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Method to reset settings to defaults
    private void ResetSettings()
    {
        PlayerPrefs.DeleteKey("ResolutionIndex");
        PlayerPrefs.DeleteKey("QualityLevel");
        PlayerPrefs.DeleteKey("Fullscreen");

        LoadSettings(); // Reload settings to apply defaults
    }
}
