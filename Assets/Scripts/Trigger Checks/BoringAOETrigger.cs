using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoringAOETrigger : MonoBehaviour
{
    private Boring enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Boring>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BulletController>() != null)
        {
            enemy.EnterAOEState();
        }
    }
}
