using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightTrigger : MonoBehaviour
{
    public GameObject lightRing;
    public GameObject luna;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            lightRing.SetActive(true);
            luna.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightRing.SetActive(false); 
            luna.SetActive(false);
        }
    }
}