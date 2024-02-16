using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooFarIShoot : MonoBehaviour
{
    private Sheriff s;

    void Start()
    {
        s = GetComponentInParent<Sheriff>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("pew");
            s.startshooting();
        }
        
    }

}
