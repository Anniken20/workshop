using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroScript : MonoBehaviour
{
    public GameObject target;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            target = collider.gameObject;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            target = null;
        }
    }

}
