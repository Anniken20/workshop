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
    public AudioClip audioClip;

    private void Start()
    {
        if (onStart)
        {
            Change();
        }
    }

    public void Change()
    {
        //new record longest single unity line
        AudioManager.main.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().clip = audioClip;
    }
}
