using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooCloseTrigger : MonoBehaviour
{
    private Sheriff s;
    private Diana d;

    void Start()
    {
        s = GetComponentInParent<Sheriff>();
        d = GetComponentInParent<Diana>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("bye");
            s.runaway();
            if(d != null)
            {
                d.stateMachine.ChangeState(d.evadeState);
            }
            
        }
        
    }

}
