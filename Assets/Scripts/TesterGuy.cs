using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterGuy : MonoBehaviour
{
    public AudioClip dialogueTest;

    private void Start()
    {
        StartCoroutine(AudioTestRoutine());
    }

    private IEnumerator AudioTestRoutine()
    {
        while (true)
        {
            AudioManager.main.Play(AudioManager.AudioSourceChannel.Dialogue, dialogueTest);

            //wait 5 seconds before continuing loop
            yield return new WaitForSeconds(5);
        }
    }
}
