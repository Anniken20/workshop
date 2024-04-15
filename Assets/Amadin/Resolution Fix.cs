using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionFix : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropDown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefresRate;
    private int currentResolutionIndex = 0;

    [System.Obsolete]
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        
        resolutionDropDown.ClearOptions();
        currentRefresRate = Screen.currentResolution.refreshRate;

        for(int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefresRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for(int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate +" Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width  && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
    /*public void Change()
    {
        Screen.fullScreen = !Screen.fullScreen;
        print("Changed screen mode");
    }*/
}
