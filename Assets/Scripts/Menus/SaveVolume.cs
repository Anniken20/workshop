using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct SliderAndLabel
{
    public TMP_Text label;
    public Slider slider;
    //public Toggle muteBox;
}

public class SaveVolume : MonoBehaviour
{
    public SliderAndLabel master;
    public SliderAndLabel music;
    public SliderAndLabel sfx;
    public SliderAndLabel dialogue;
    public SliderAndLabel ambience;

    public void OnEnable()
    {
        float v1 = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 0.8f);
        master.label.text = (v1 * 100).ToString();
        master.slider.value = v1;
        //if (!master.muteBox.isOn) { AudioManager.main.}

        v1 = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 0.8f);
        music.label.text = (v1 * 100).ToString();
        music.slider.value = v1;

        v1 = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 0.8f);
        sfx.label.text = (v1 * 100).ToString();
        sfx.slider.value = v1;

        v1 = PlayerPrefs.GetFloat(AudioManager.DIALOGUE_KEY, 0.8f);
        dialogue.label.text = (v1 * 100).ToString();
        dialogue.slider.value = v1;

        v1 = PlayerPrefs.GetFloat(AudioManager.AMBIENCE_KEY, 0.8f);
        ambience.label.text = (v1 * 100).ToString();
        ambience.slider.value = v1;
    }

    public void UpdateMaster()
    {
        master.label.text = (master.slider.value * 100).ToString();
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, master.slider.value);
        AudioManager.main.LoadMasterVolume();
    }

    public void UpdateMusic()
    {
        music.label.text = (music.slider.value * 100).ToString();
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, music.slider.value);
        AudioManager.main.LoadMusicVolume();
    }

    public void UpdateSFX()
    {
        sfx.label.text = (sfx.slider.value * 100).ToString();
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfx.slider.value);
        AudioManager.main.LoadSFXVolume();
    }

    public void UpdateDialogue()
    {
        dialogue.label.text = (dialogue.slider.value * 100).ToString();
        PlayerPrefs.SetFloat(AudioManager.DIALOGUE_KEY, dialogue.slider.value);
        AudioManager.main.LoadDialogueVolume();
    }

    public void UpdateAmbience()
    {
        ambience.label.text = (ambience.slider.value * 100).ToString();
        PlayerPrefs.SetFloat(AudioManager.AMBIENCE_KEY, ambience.slider.value);
        AudioManager.main.LoadAmbienceVolume();
    }

    /*public void ToggleMuteMaster() { master.muteBox.isOn = !master.muteBox.isOn; 
        PlayerPrefs.SetInt("MasterMuted", 1); }*/

}
