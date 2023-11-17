using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleSettings : MonoBehaviour
{
    private Toggle subtitleToggle;
    public void ToggleSubtitles()
    {
        //flip from 0 to 1
        if(subtitleToggle.isOn)
        {
            PlayerPrefs.SetInt("SubtitlesOn", 1);
        } else
        {
            PlayerPrefs.SetInt("SubtitlesOn", 0);
        }
    }

    private void OnEnable()
    {
        subtitleToggle = GetComponent<Toggle>();
        subtitleToggle.isOn = PlayerPrefs.GetInt("SubtitlesOn", 1) == 1;
    }
}
