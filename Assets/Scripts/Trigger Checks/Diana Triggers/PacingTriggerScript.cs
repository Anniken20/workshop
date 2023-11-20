using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacingTriggerScript : MonoBehaviour
{
    public Diana diana;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            diana.BeginPacing();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           diana.StartShooting();
        }
    }
}
