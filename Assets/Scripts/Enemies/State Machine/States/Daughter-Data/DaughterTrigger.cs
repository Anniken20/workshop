using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaughterTrigger : MonoBehaviour
{
    [SerializeField] GraveSelection g;
    private bool canTrigger = true;

    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canTrigger)
        {
            g.ActivateGrave();
            canTrigger = false;
        }
    }
}
