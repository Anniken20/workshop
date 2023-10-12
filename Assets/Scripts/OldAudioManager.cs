using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OldAudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioMixer mixer;

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";
    public const string DIALOGUE_KEY = "dialogueVolume";
    
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_DIALOGUE = "DialogueVolume";

    public static float musicVolume;

    public AudioSource mainMusic;
    public AudioSource receptionMusic;
    public AudioSource administrationMusic;
    public AudioSource laboratoriesMusic;
    public AudioSource officeMusic;

    public float musicVolume2;
    public float sfxVolume;
    public float dialogueVolume;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider dialogueSlider;
    
    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        
        /*
        if(instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        

        LoadVolume();
        */
        
        if (scene.name == "MainMenu")
        {
            mainMusic.Play();
            receptionMusic.Stop();
            administrationMusic.Stop();
            laboratoriesMusic.Stop();
            officeMusic.Stop();
        }

        if (scene.name == "Cutscene Room")
        {
            mainMusic.Play();
            receptionMusic.Stop();
            administrationMusic.Stop();
            laboratoriesMusic.Stop();
            officeMusic.Stop();
        }

        if(scene.name == "Reception")
        {
            mainMusic.Stop();
            receptionMusic.Play();
            administrationMusic.Stop();
            laboratoriesMusic.Stop();
            officeMusic.Stop(); 
        }

        if(scene.name == "Administration")
        {
            mainMusic.Stop();
            receptionMusic.Stop();
            administrationMusic.Play();
            laboratoriesMusic.Stop();
            officeMusic.Stop(); 
        }

        if(scene.name == "Laboratory")
        {
            mainMusic.Stop();
            receptionMusic.Stop();
            administrationMusic.Stop();
            laboratoriesMusic.Play();
            officeMusic.Stop(); 
        }

        if(scene.name == "Office")
        {
            mainMusic.Stop();
            receptionMusic.Stop();
            administrationMusic.Stop();
            laboratoriesMusic.Stop();
            officeMusic.Play(); 
        }
    }
    
    void Update()
    {
        musicVolume2 = musicSlider.value;
        sfxVolume = sfxSlider.value;
        dialogueVolume = dialogueSlider.value;
    }

    /*
    void LoadVolume() //Volume saved in VolumeSettings script
    {
        musicVolume2 = PlayerPrefs.GetFloat(MUSIC_KEY,1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY,1f);
        dialogueVolume = PlayerPrefs.GetFloat(DIALOGUE_KEY,1f);

        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume2) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_DIALOGUE, Mathf.Log10(dialogueVolume) * 20);
    }
    */
}
