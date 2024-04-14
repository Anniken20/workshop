using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

/* AudioManager script
 * 
 * 
 * Started by Anniken and remade for GMHN by Caden
 * 10/12/23
 * 
 */

public class AudioManager : MonoBehaviour
{
    //Singleton
    [HideInInspector] public static AudioManager main;

    [SerializeField] private AudioMixer mixer;

    public AudioSource musicAudio;
    public AudioSource sfxAudio;
    public AudioSource dialogueAudio;
    public AudioSource ambienceAudio;

    [Header("Effects")]
    [SerializeField] private AudioLowPassFilter musicLPFilter;

    //keys for player prefs ------------------------------------------------
    [HideInInspector] public const string MASTER_KEY = "masterVolume";
    [HideInInspector] public const string MUSIC_KEY = "musicVolume";
    [HideInInspector] public const string SFX_KEY = "sfxVolume";
    [HideInInspector] public const string DIALOGUE_KEY = "dialogueVolume";
    [HideInInspector] public const string AMBIENCE_KEY = "ambienceVolume";

    [HideInInspector] public const string MASTER_UNMUTED = "masterUnmuted";
    [HideInInspector] public const string MUSIC_UNMUTED = "musicUnmuted";
    [HideInInspector] public const string SFX_UNMUTED = "sfxUnmuted";
    [HideInInspector] public const string DIALOGUE_UNMUTED = "dialogueUnmuted";
    [HideInInspector] public const string AMBIENCE_UNMUTED = "ambienceUnmuted";

    //enum used when passing in calls from other scripts
    public enum AudioSourceChannel
    {
        Music,
        SFX,
        Dialogue,
        Ambience
    }

    private void Awake()
    {        
        //Singleton behavior
        if(main != null)
            Destroy(gameObject);
        else
        {
            main = this;
        }   
    }

    private void Start()
    {
        //this goes in Start because Unity warns of unexpected behavior
        //when changing mixer values in Awake
        LoadMutedVolumes();
    }

    public void Play(AudioSourceChannel channel, AudioClip clip, float volume = 1.0f)
    {
        switch (channel)
        {
            case AudioSourceChannel.Music:
                musicAudio.PlayOneShot(clip, volume);
                break;
            case AudioSourceChannel.SFX:
                sfxAudio.PlayOneShot(clip, volume);
                break;
            case AudioSourceChannel.Dialogue:
                dialogueAudio.PlayOneShot(clip, volume);
                break;
            case AudioSourceChannel.Ambience:
                ambienceAudio.PlayOneShot(clip, volume);
                break;
            default:
                Debug.LogWarning("Audio passed in to nonexistent channel in AudioManager!");
                break;
        }
    }

    #region Settings
    public void LoadVolume() 
    {
        LoadMasterVolume();
        LoadMusicVolume();
        LoadSFXVolume();
        LoadDialogueVolume();
        LoadAmbienceVolume();
    }

    public void LoadMasterVolume()
    {
        if (PlayerPrefs.GetInt(MASTER_UNMUTED, 1) == 0) return;
        mixer.SetFloat(MASTER_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(MASTER_KEY, 0.8f)) * 70f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(MASTER_KEY), 0))
            mixer.SetFloat(MASTER_KEY, -80f);
    }

    public void LoadMusicVolume()
    {
        if (PlayerPrefs.GetInt(MUSIC_UNMUTED, 1) == 0) return;
        mixer.SetFloat(MUSIC_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(MUSIC_KEY, 0.8f)) * 70f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(MUSIC_KEY), 0))
            mixer.SetFloat(MUSIC_KEY, -80f);
            
    }

    public void LoadSFXVolume()
    {
        if (PlayerPrefs.GetInt(SFX_UNMUTED, 1) == 0) return;
        mixer.SetFloat(SFX_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(SFX_KEY, 0.8f)) * 70f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(SFX_KEY), 0))
            mixer.SetFloat(SFX_KEY, -80f);
    }

    public void LoadDialogueVolume()
    {
        if (PlayerPrefs.GetInt(DIALOGUE_UNMUTED, 1) == 0) return;
        mixer.SetFloat(DIALOGUE_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(DIALOGUE_KEY, 0.8f)) * 70f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(DIALOGUE_KEY), 0))
            mixer.SetFloat(DIALOGUE_KEY, -80f);
    }

    public void LoadAmbienceVolume()
    {
        if (PlayerPrefs.GetInt(AMBIENCE_UNMUTED, 1) == 0) return;
        mixer.SetFloat(AMBIENCE_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(AMBIENCE_KEY, 0.8f)) * 70f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(AMBIENCE_KEY), 0))
            mixer.SetFloat(AMBIENCE_KEY, -80f);
    }

    public void LoadMasterToggle()
    {
        if(PlayerPrefs.GetInt(MASTER_UNMUTED, 1) == 0) {
            mixer.SetFloat(MASTER_KEY, -9999);
        } else
        {
            LoadMasterVolume();
        }
    }

    public void LoadMusicToggle()
    {
        if (PlayerPrefs.GetInt(MUSIC_UNMUTED, 1) == 0)
        {
            mixer.SetFloat(MUSIC_KEY, -9999);
        }
        else
        {
            LoadMusicVolume();
        }
    }

    public void LoadSFXToggle()
    {
        if (PlayerPrefs.GetInt(SFX_UNMUTED, 1) == 0)
        {
            mixer.SetFloat(SFX_KEY, -9999);
        }
        else
        {
            LoadSFXVolume();
        }
    }

    public void LoadDialogueToggle()
    {
        if (PlayerPrefs.GetInt(DIALOGUE_UNMUTED, 1) == 0)
        {
            mixer.SetFloat(DIALOGUE_KEY, -9999);
        }
        else
        {
            LoadDialogueVolume();
        }
    }

    public void LoadAmbienceToggle()
    {
        if (PlayerPrefs.GetInt(AMBIENCE_UNMUTED, 1) == 0)
        {
            mixer.SetFloat(AMBIENCE_KEY, -9999);
        }
        else
        {
            LoadAmbienceVolume();
        }
    }

    public void LoadMutedVolumes()
    {
        LoadMasterToggle();
        LoadMusicToggle();
        LoadSFXToggle();
        LoadAmbienceToggle();
        LoadDialogueToggle();
    }
    #endregion

    #region EffectsMethods
    public void SetMusicLowPassFilter(int newValue = 22000, float time = 1f)
    {
        newValue = Mathf.Clamp(newValue, 0, 22000);
        DOTween.To(() => musicLPFilter.cutoffFrequency, x => musicLPFilter.cutoffFrequency = x, newValue, time);
    }

    #endregion
}
