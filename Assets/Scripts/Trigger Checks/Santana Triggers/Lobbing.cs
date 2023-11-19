using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobbing : MonoBehaviour
{
    public Santana santana;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            santana.BeginLob();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            santana.StopLob();
        }
    }
}