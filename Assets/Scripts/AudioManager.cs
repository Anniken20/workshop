using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    //keys for player prefs ------------------------------------------------
    [HideInInspector] public const string MASTER_KEY = "masterVolume";
    [HideInInspector] public const string MUSIC_KEY = "musicVolume";
    [HideInInspector] public const string SFX_KEY = "sfxVolume";
    [HideInInspector] public const string DIALOGUE_KEY = "dialogueVolume";
    [HideInInspector] public const string AMBIENCE_KEY = "ambienceVolume";

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
            DontDestroyOnLoad(gameObject);
            main = this;
        }   
    }

    private void Start()
    {
        //this goes in Start because Unity warns of unexpected behavior
        //when changing mixer values in Awake
        LoadVolume();
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
        mixer.SetFloat(MASTER_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(MASTER_KEY, 0.8f)) * 20f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(MASTER_KEY), 0))
            mixer.SetFloat(MASTER_KEY, -80f);
    }

    public void LoadMusicVolume()
    {
        mixer.SetFloat(MUSIC_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(MUSIC_KEY, 0.8f)) * 20f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(MUSIC_KEY), 0))
            mixer.SetFloat(MUSIC_KEY, -80f);
            
    }

    public void LoadSFXVolume()
    {
        mixer.SetFloat(SFX_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(SFX_KEY, 0.8f)) * 20f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(SFX_KEY), 0))
            mixer.SetFloat(SFX_KEY, -80f);
    }

    public void LoadDialogueVolume()
    {
        mixer.SetFloat(DIALOGUE_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(DIALOGUE_KEY, 0.8f)) * 20f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(DIALOGUE_KEY), 0))
            mixer.SetFloat(DIALOGUE_KEY, -80f);
    }

    public void LoadAmbienceVolume()
    {
        mixer.SetFloat(AMBIENCE_KEY,
            Mathf.Log10(PlayerPrefs.GetFloat(AMBIENCE_KEY, 0.8f)) * 20f);
        if (Mathf.Approximately(PlayerPrefs.GetFloat(AMBIENCE_KEY), 0))
            mixer.SetFloat(AMBIENCE_KEY, -80f);
    }
}
