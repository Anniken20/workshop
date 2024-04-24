using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureKeeperAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;

    void Start()
    {
        anim.SetBool("Crying", false);
        anim.SetBool("Idle", true);
    }

    public void CallBirds()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Crying", true);
        StartCoroutine(CowerDelay());

    }

    public IEnumerator CowerDelay()
    {
        yield return new WaitForSeconds(5f);
        anim.SetBool("Crying", false);
        anim.SetBool("Idle", true);
    }

}
