using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AudioClip pickUpSound;
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupAction();
            PlayPickupSound();
            Destroy(gameObject);
        }
    }

    protected virtual void PickupAction()
    {
        //overridden
    }

    protected void PlayPickupSound()
    {
        if (pickUpSound != null)
        {
            AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, pickUpSound);
        }
    }
}