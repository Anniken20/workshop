using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureKeeperAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip projectileSFX;

    void Start()
    {
        anim.SetBool("Crying", false);
        anim.SetBool("Throw", false);
        anim.SetBool("Idle", true);
    }

    public void CallBirds()
    {
        audioSource.PlayOneShot(projectileSFX);
        if (anim.GetBool("Throw") == false)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Crying", true);
            StartCoroutine(CowerDelay());
        }
    }

    public IEnumerator CowerDelay()
    {
        yield return new WaitForSeconds(2.5f);
        anim.SetBool("Crying", false);
        anim.SetBool("Throw", false);
        anim.SetBool("Idle", true);
    }

}
