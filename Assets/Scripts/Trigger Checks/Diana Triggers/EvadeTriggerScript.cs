using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeTriggerScript : MonoBehaviour
{
    public Diana diana;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            diana.BeginEvade();
        }
    }
        void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           diana.BeginPacing();
        }
    }
}
