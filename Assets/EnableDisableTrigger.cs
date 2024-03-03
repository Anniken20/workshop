using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableTrigger : BaseTrigger
{
    public GameObject[] toInvis; 

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HitEffect();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < toInvis.Length; i++)
            {
                toInvis[i].gameObject.SetActive(true);
            }
        }
    }
    protected override void HitEffect()
    {

        for (int i = 0; i < toInvis.Length; i++)
        {
            toInvis[i].gameObject.SetActive(false);
        }
    }
}
