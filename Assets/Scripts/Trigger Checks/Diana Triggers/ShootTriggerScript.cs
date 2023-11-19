using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTriggerScript : MonoBehaviour
{
    public Diana diana;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            diana.StartShooting();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            diana.StopShooting();
        }
    }
}
