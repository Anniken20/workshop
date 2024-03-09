using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialiteMoveTrigger : MonoBehaviour
{
    private Socialite s;
    private bool canTrigger = true;
    public bool pressed;
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
    void Update()
    {
        if (pressed)
        {
            s.StartMove();
            pressed = false;
        }
    }
}
