using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobAttackTriggerScript : MonoBehaviour
{
    public Diana diana;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            diana.BeginLob();
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
