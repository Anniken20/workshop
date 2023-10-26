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
    public DialogueClip[] unpromptedDialogues;
    [Tooltip("Minimum time between playing dialogue clips")]
    public float rndLowBound;
    [Tooltip("Maximum time between playing dialogue clips")]
    public float rndHighBound;

    private bool spatialMixing;

    private void Start()
    {
        spatialMixing = audioSource.spatialBlend > 0;
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

        StartCoroutine(WriteToScreen(clip));
    }

    private IEnumerator WriteToScreen(DialogueClip clip)
    {
        subtitleText.text = clip.speaker + ": " + clip.text;
        subtitleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(clip.duration);

        subtitleText.text = "";
        subtitleText.gameObject.SetActive(false);
        //subtitleText.color.DOTween()

        // Tween a float called myFloat to 52 in 1 second
        //DOTween.To(() => myFloat, x => myFloat = x, 52, 1);
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
}
