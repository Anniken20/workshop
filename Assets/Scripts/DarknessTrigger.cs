using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DarknessTrigger : MonoBehaviour
{
    public GameObject globalLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            globalLight.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            globalLight.SetActive(true);
        }
    }
}