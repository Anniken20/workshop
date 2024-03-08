using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesmanTrigger : MonoBehaviour
{
    private Salesman s;
    private bool canTrigger = true;
    private void Start()
    {
        s = GetComponentInParent<Salesman>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canTrigger)
        {
            //s.StartMove();
            s.ChaseState();
            canTrigger = false;
        }
    }
}
