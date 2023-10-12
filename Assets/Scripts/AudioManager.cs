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

    //volumes --------------------------------------------------------------
    //these are set on scene start
    //and when changing values in settings
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float sfxVolume;
    [HideInInspector] public float dialogueVolume;
    [HideInInspector] public float ambienceVolume;

    //keys for player prefs ------------------------------------------------
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
        
        LoadVolume();      
    }

    /*
    void Update()
    {
        musicVolume2 = musicSlider.value;
        sfxVolume = sfxSlider.value;
        dialogueVolume = dialogueSlider.value;
    }
    */

    public void Play(AudioSourceChannel channel, AudioClip clip)
    {
        switch (channel)
        {
            case AudioSourceChannel.Music:
                musicAudio.PlayOneShot(clip);
                break;
            case AudioSourceChannel.SFX:
                sfxAudio.PlayOneShot(clip);
                break;
            case AudioSourceChannel.Dialogue:
                dialogueAudio.PlayOneShot(clip);
                break;
            case AudioSourceChannel.Ambience:
                ambienceAudio.PlayOneShot(clip);
                break;
            default:
                Debug.LogWarning("Audio passed in to nonexistent channel in AudioManager!");
                break;
        }
    }

    //Volume saved in VolumeSettings script
    public void LoadVolume() 
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY,0.8f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY,0.8f);
        dialogueVolume = PlayerPrefs.GetFloat(DIALOGUE_KEY,0.8f);
        ambienceVolume = PlayerPrefs.GetFloat(AMBIENCE_KEY, 0.8f);

        //mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        //mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        //mixer.SetFloat(VolumeSettings.MIXER_DIALOGUE, Mathf.Log10(dialogueVolume) * 20);
        //mixer.SetFloat(VolumeSettings.MIXER_AMBIENCE, Mathf.Log10(dialogueVolume) * 20);
    }
}
