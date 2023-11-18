using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTriggerSantana : MonoBehaviour
{
    public Santana santana;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            santana.BeginAOE();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            santana.StopAOE();
        }
    }
}