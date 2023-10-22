using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonShaderToggle : MonoBehaviour
{
    public Material material;  // Reference to the material with the shader you want to toggle
    public KeyCode toggleKey = KeyCode.Space;  // The key to toggle the shader
    private bool shaderEnabled = true;  // Flag to track whether the shader is enabled
    void Update()
    {
        // Check if the toggle key is pressed
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleShader();
        }
    }

    void ToggleShader()
    {
        shaderEnabled = !shaderEnabled;  // Toggle the flag

        if (shaderEnabled)
        {
            // Enable the shader
            material.EnableKeyword("_ENABLE_SHADER");
        }
        else
        {
            // Disable the shader
            material.DisableKeyword("_ENABLE_SHADER");
        }
    }
}