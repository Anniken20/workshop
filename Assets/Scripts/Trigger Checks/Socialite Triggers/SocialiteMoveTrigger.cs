using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialiteMoveTrigger : MonoBehaviour
{
    private Socialite s;
    private bool canTrigger = true;
    private void Start()
    {
        s = GetComponentInParent<Socialite>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canTrigger)
        {
            s.StartMove();
            canTrigger = false;
        }

    }
}
