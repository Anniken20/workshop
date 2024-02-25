using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaughterTrigger : MonoBehaviour
{
    private Daughter d;
    private bool canTrigger = true;

    private void Start()
    {
        d = GetComponentInParent<Daughter>();
        //Debug.Log(d.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canTrigger)
        {
            d.SelectGrave();
            canTrigger = false;
        }
    }
}
