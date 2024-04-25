using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureKeeperAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip projectileSFX;

    private Coroutine cowerRoutine;

    void OnEnable()
    {
        anim.SetBool("Crying", false);
        anim.SetBool("Idle", true);
    }

    public void CallBirds()
    {
        audioSource.PlayOneShot(projectileSFX);
        if (anim.GetBool("Throw") == false)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Crying", true);
            if (cowerRoutine != null) StopCoroutine(cowerRoutine);
            cowerRoutine = StartCoroutine(CowerDelay());
        }
    }

    public IEnumerator CowerDelay()
    {
        yield return new WaitForSeconds(2.5f);
        anim.SetBool("Crying", false);
        Debug.Log("cower delay set throw false");
        anim.SetBool("Throw", false);
        anim.SetBool("Idle", true);
    }

}
