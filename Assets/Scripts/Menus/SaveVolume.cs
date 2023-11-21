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
    public Toggle muteBox;
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
        master.label.text = ((int)(100 * v1)).ToString();
        master.slider.value = v1;
        //if (!master.muteBox.isOn) { AudioManager.main.}

        v1 = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 0.8f);
        music.label.text = ((int)(100 * v1)).ToString();
        music.slider.value = v1;

        v1 = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 0.8f);
        sfx.label.text = ((int)(100 * v1)).ToString();
        sfx.slider.value = v1;

        v1 = PlayerPrefs.GetFloat(AudioManager.DIALOGUE_KEY, 0.8f);
        dialogue.label.text = ((int)(100 * v1)).ToString();
        dialogue.slider.value = v1;

        v1 = PlayerPrefs.GetFloat(AudioManager.AMBIENCE_KEY, 0.8f);
        ambience.label.text = ((int)(100 * v1)).ToString();
        ambience.slider.value = v1;

        SetMuteBoxes();
    }

    public void UpdateMaster()
    {
        master.label.text = ((int)(100 * master.slider.value)).ToString();
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, master.slider.value);
        AudioManager.main.LoadMasterVolume();
    }

    public void UpdateMusic()
    {
        music.label.text = ((int)(100 * music.slider.value)).ToString();
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, music.slider.value);
        AudioManager.main.LoadMusicVolume();
    }

    public void UpdateSFX()
    {
        sfx.label.text = ((int)(100 * sfx.slider.value)).ToString();
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfx.slider.value);
        AudioManager.main.LoadSFXVolume();
    }

    public void UpdateDialogue()
    {
        dialogue.label.text = ((int)(100 * dialogue.slider.value)).ToString();
        PlayerPrefs.SetFloat(AudioManager.DIALOGUE_KEY, dialogue.slider.value);
        AudioManager.main.LoadDialogueVolume();
    }

    public void UpdateAmbience()
    {
        ambience.label.text = ((int)(100 * ambience.slider.value)).ToString();
        PlayerPrefs.SetFloat(AudioManager.AMBIENCE_KEY, ambience.slider.value);
        AudioManager.main.LoadAmbienceVolume();
    }

    private void SetMuteBoxes()
    {
        master.muteBox.isOn = PlayerPrefs.GetInt(AudioManager.MASTER_UNMUTED, 1) == 1;
        music.muteBox.isOn = PlayerPrefs.GetInt(AudioManager.MUSIC_UNMUTED, 1) == 1;
        sfx.muteBox.isOn = PlayerPrefs.GetInt(AudioManager.SFX_UNMUTED, 1) == 1;
        dialogue.muteBox.isOn = PlayerPrefs.GetInt(AudioManager.DIALOGUE_UNMUTED, 1) == 1;
        ambience.muteBox.isOn = PlayerPrefs.GetInt(AudioManager.AMBIENCE_UNMUTED, 1) == 1;
    }

    public void ToggleMuteMaster() {
        PlayerPrefs.SetInt(AudioManager.MASTER_UNMUTED, master.muteBox.isOn ? 1 : 0);
        AudioManager.main.LoadMasterToggle();
    }

    public void ToggleMuteMusic()
    {
        PlayerPrefs.SetInt(AudioManager.MUSIC_UNMUTED, music.muteBox.isOn ? 1 : 0);
        AudioManager.main.LoadMusicToggle();
    }

    public void ToggleMuteSFX()
    {
        PlayerPrefs.SetInt(AudioManager.SFX_UNMUTED, sfx.muteBox.isOn ? 1 : 0);
        AudioManager.main.LoadSFXToggle();
    }

    public void ToggleMuteDialogue()
    {
        PlayerPrefs.SetInt(AudioManager.DIALOGUE_UNMUTED, dialogue.muteBox.isOn ? 1 : 0);
        AudioManager.main.LoadDialogueToggle();
    }

    public void ToggleMuteAmbience()
    {
        PlayerPrefs.SetInt(AudioManager.AMBIENCE_UNMUTED, ambience.muteBox.isOn ? 1 : 0);
        AudioManager.main.LoadAmbienceToggle();
    }


}
