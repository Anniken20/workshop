using UnityEngine;

public class GraphicsSettingsController : MonoBehaviour
{
    public GraphicsData graphicsData;

    public void SetResolution(int width, int height)
    {
        graphicsData.resolutionWidth = width;
        graphicsData.resolutionHeight = height;
        ApplyGraphicsSettings();
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        graphicsData.fullscreen = isFullscreen;
        ApplyGraphicsSettings();
    }

    public void SetQualityLevel(int level)
    {
        graphicsData.qualityLevel = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1);
        ApplyGraphicsSettings();
    }

    private void ApplyGraphicsSettings()
    {
        Screen.SetResolution(graphicsData.resolutionWidth, graphicsData.resolutionHeight, graphicsData.fullscreen);
        QualitySettings.SetQualityLevel(graphicsData.qualityLevel);
    }
}
