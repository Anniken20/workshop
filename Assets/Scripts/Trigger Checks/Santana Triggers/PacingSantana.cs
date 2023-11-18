using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacingSantana : MonoBehaviour
{
    public Santana santana;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            santana.BeginPacing();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            santana.StopPacing();
        }
    }
}
