using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Helper script to quickly change the music playing in a level
 * 
 * Caden Henderson
 * 12/5/23
 */
public class ChangeMusic : MonoBehaviour
{
    public bool onStart = true;
    public bool stopMusic = false;
    public AudioClip audioClip;

    private AudioSource musicSource;

    private void Start()
    {
        if(stopMusic)
        {
            musicSource = AudioManager.main.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
            musicSource.Stop();
        } else
        {
            musicSource = AudioManager.main.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
            musicSource.Play();
        }
        if (onStart)
        {
            Change();
        }
    }

    public void Change()
    {
        //new record longest single unity line
        //old line: AudioManager.main.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().clip = audioClip;
        musicSource = AudioManager.main.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        if (musicSource.clip != audioClip && audioClip != null) musicSource.clip = audioClip;


    }
}
