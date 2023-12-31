using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShotSound : MonoBehaviour, IShootable
{ 
    [Tooltip("This will play when the bullet ricochets")]
    public AudioClip ricochetClip;
    [Tooltip("This will play when the bullet passes through the surface")]
    public AudioClip throughClip;
    private AudioSource audioSource;
    public void OnShot(BulletController bullet)
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (bullet.HasBouncesRemaining())
        {
            if(ricochetClip != null)
                audioSource.PlayOneShot(ricochetClip);
        } else
        {
            if (throughClip != null)
                audioSource.PlayOneShot(throughClip);
        }
    }
}
