using UnityEngine;
using UnityEngine.UI;

public class CursorSensitivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Button clearButton; // Reference to the clear button in the UI

    private const string SensitivityKey = "CursorSensitivity";

    private AimController aimController;

    private void Start()
    {
        // Load sensitivity from player prefs or use default if not set
        float sensitivity = PlayerPrefs.GetFloat(SensitivityKey, 10f);
        sensitivitySlider.value = sensitivity;
        OnSensitivityChanged(sensitivity);

        // Find the AimController in the scene if available
        aimController = FindObjectOfType<AimController>();

        // Subscribe to the clear button's click event
        clearButton.onClick.AddListener(OnClearButtonClicked);
    }

    private void OnSensitivityChanged(float sensitivity)
    {
        // If AimController is present, update cursor sensitivity
        if (aimController != null)
        {
            aimController.SetCursorSensitivity(sensitivity);
        }
        else
        {
            Debug.LogWarning("AimController not found in the scene. Cursor sensitivity not updated.");
        }

        // Save sensitivity to player prefs
        PlayerPrefs.SetFloat(SensitivityKey, sensitivity);
        PlayerPrefs.Save();
    }

    private void OnClearButtonClicked()
    {
        // Clear sensitivity player prefs
        PlayerPrefs.DeleteKey(SensitivityKey);
        PlayerPrefs.Save();

        // Reset slider value
        sensitivitySlider.value = 10f;

        // If AimController is present, reset cursor sensitivity to default
        if (aimController != null)
        {
            aimController.SetCursorSensitivity(10f);
        }
    }
}
