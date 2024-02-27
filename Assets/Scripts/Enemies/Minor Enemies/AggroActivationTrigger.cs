using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AggroActivationTrigger : MonoBehaviour
{
    //make sure functions are override instead of void
    public GameObject[] aggroArray;
    public bool deactivateOnEnter;

    private void Start()
    {
            foreach (GameObject aggro in aggroArray)
            {
                aggro.SetActive(false);
            }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(deactivateOnEnter)
            {
                foreach (GameObject aggro in aggroArray)
                {
                    aggro.SetActive(false);
                }
               
            }
            else
            {
                foreach (GameObject aggro in aggroArray)
                {
                    aggro.SetActive(true);
                }
            }

        }
    }
}
