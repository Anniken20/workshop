using UnityEngine;
using UnityEngine.UI;

public class CursorSensitivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Button clearButton;

    private const string SensitivityKey = "CursorSensitivity";
    private const float DefaultSensitivity = 25f;
    public float cursorSensitivity;

    private AimController aimController;

    private void Start()
    {
        // Load sensitivity from player prefs or use default if not set
        cursorSensitivity = PlayerPrefs.GetFloat(SensitivityKey, DefaultSensitivity);
        sensitivitySlider.value = cursorSensitivity;

        // Subscribe to the slider's value changed event
        sensitivitySlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Subscribe to the clear button's click event
        clearButton.onClick.AddListener(OnClearButtonClicked);

        aimController = FindObjectOfType<AimController>();

        //Debug.Log("Start - Sensitivity loaded: " + cursorSensitivity);
    }

    private void OnSliderValueChanged(float sensitivity)
    {
        // Call SetCursorSensitivity function
        SetCursorSensitivity(sensitivity);

        // Save sensitivity to player prefs
        PlayerPrefs.SetFloat(SensitivityKey, sensitivity);
        PlayerPrefs.Save();

        //Debug.Log("Sensitivity changed: " + sensitivity);
    }

    private void SetCursorSensitivity(float sensitivity)
    {
        // This function sets the cursor sensitivity
        cursorSensitivity = sensitivity;
        aimController.SetCursorSensitivity(cursorSensitivity);
        //Cursor.sensitivity = sensitivity;
    }

    private void OnClearButtonClicked()
    {
        // Clear sensitivity player prefs
        PlayerPrefs.DeleteKey(SensitivityKey);
        PlayerPrefs.Save();

        // Reset slider value to default sensitivity
        sensitivitySlider.value = DefaultSensitivity;

        // Reset cursor sensitivity to default
        cursorSensitivity = DefaultSensitivity;

        Debug.Log("Sensitivity cleared. Slider reset to default: " + DefaultSensitivity);
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * cursorSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cursorSensitivity * Time.deltaTime;
    }
}
