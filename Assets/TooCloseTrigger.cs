using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooCloseTrigger : MonoBehaviour
{
    private Sheriff s;

    void Start()
    {
        s = GetComponentInParent<Sheriff>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("yep");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hello");
            s.runaway();
        }
        
    }

}
