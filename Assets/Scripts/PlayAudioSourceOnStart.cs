using UnityEngine;
using System.Collections;

/*
 * To prevent playing from audio sources before volume settings have loaded and set.
 * Technically making it play 2 frames after Start just to prevent potential race conditions
 * and annoying *click* sounds of the audios having their volume changed. 
 * 
 * This may be the last new script I make for this game.
 * It's been an honor! :)
 * - Caden
 * 4/26/24
 */

public class PlayAudioSourceOnStart : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DelayedSet());
    }

    private IEnumerator DelayedSet()
    {
        yield return null;
        yield return null;
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null) audioSource.Play();
    }

}
