using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

/* This script manages different forms of speech audio.
 * Includes subtitle settings, too.
 * 
 * 
 * 
 * Caden Henderson
 * 10/26/23
 */

[System.Serializable]
public struct DialogueClip
{
    public AudioClip speechClip;
    public string text;
    public string speaker;
    public float duration;
}

public class DialogueController : MonoBehaviour
{
    [Header("Necessary")]
    public DialogueClip dialogue;
    public AudioSource audioSource;
    public TMP_Text subtitleText;

    [Header("Unprompted Dialogue")]
    [Tooltip("Will randomly play a piece of dialogue from the list setup below")]
    public bool playUnprompted;
    [Tooltip("Minimum time between playing dialogue clips")]
    public float rndLowBound;
    [Tooltip("Maximum time between playing dialogue clips")]
    public float rndHighBound;
    [Tooltip("Attach the player if using spatial mixing on the Audio Source")]
    public GameObject sceneListener;
    public DialogueClip[] unpromptedDialogues;
    

    private bool spatialMixing;
    private SubtitleDisplayController subtitleDisplayController;

    private void Start()
    {
        spatialMixing = audioSource.spatialBlend > 0;
        subtitleDisplayController = subtitleText.GetComponent<SubtitleDisplayController>();

        if (playUnprompted) 
            StartCoroutine(UnpromptedDialogueRoutine());
    }

    //default. can be called by unity events
    public void PlayDialogue()
    {
        PlayDialogue(dialogue);
    }

    //override for non-default clip
    public void PlayDialogue(DialogueClip clip)
    {
        audioSource.clip = clip.speechClip;
        audioSource.Play();

        //if audio is audible, write to subtitles
        if(((!spatialMixing) || InSpatialRange()) && PlayerPrefs.GetInt("SubtitlesOn", 1) == 1) StartCoroutine(WriteToScreen(clip));
    }

    private IEnumerator WriteToScreen(DialogueClip clip)
    {
        subtitleDisplayController.LoadMessage(clip.speaker + ": " + clip.text);
        yield return new WaitForSeconds(clip.duration);
        subtitleDisplayController.UnloadMessage(clip.speaker + ": " + clip.text);
    }

    private IEnumerator UnpromptedDialogueRoutine()
    {
        while (true)
        {
            //wait random time
            yield return new WaitForSeconds(Random.Range(rndLowBound, rndHighBound));

            //play 1 of the random clips
            PlayDialogue(unpromptedDialogues[(int)Random.Range(0, unpromptedDialogues.Length)]);
        }
    }

    //returns true if the player can hear this audio
    //requires reference to player
    private bool InSpatialRange()
    {
        return Vector3.Distance(gameObject.transform.position,
            sceneListener.transform.position) < audioSource.maxDistance;
    }
}
