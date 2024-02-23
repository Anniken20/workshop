using UnityEngine;
using UnityEngine.UI;

public class SimpleColorPicker : MonoBehaviour
{
    public DialogueSettingsController dialogueSettingsController; // Reference to the DialogueSettingsController script

    public void SetColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < dialogueSettingsController.colorOptions.Length)
        {
            Color selectedColor = dialogueSettingsController.colorOptions[colorIndex];

            // Update the color display image with the selected color
            dialogueSettingsController.SetTextColor(selectedColor);
        }
    }
}
