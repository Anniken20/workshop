using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script that works with the BulletController and GunController to play sounds
 * 
 * Caden Henderson
 * 11/1/23
 */

[System.Serializable]
public struct NameAndClip
{
    public string name;
    public AudioClip clip;
}

public class GunAudioController : MonoBehaviour
{
    [Header("Volumes")]
    public float fireVolume;
    public float ricochetVolume;
    public float throughSurfaceVolume;
    public float lunaSFXVolume;

    [Header("Clips")]
    public NameAndClip[] fireClips;
    public NameAndClip[] ricochetClips;
    public NameAndClip[] collisionClips;
    public NameAndClip[] lunaClips;

    public void PlayFire()
    {
        AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, 
            fireClips[(int)Random.Range(0, fireClips.Length)].clip, fireVolume);
    }
    public void PlayRicochet(string surface = "", int bounce = -1)
    {
        //specifying specific surface
        List<NameAndClip> properClips = new();
        bool surfaceSpecific = surface != "";
        if (surfaceSpecific)
        {
            for (int i = 0; i < ricochetClips.Length; ++i)
            {
                if (ricochetClips[i].name.Contains(surface))
                    properClips.Add(ricochetClips[i]);
            }
        }

        //default play random sound
        if(bounce == -1)
        {
            if(surfaceSpecific)
                AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX,
                properClips[(int)Random.Range(0, properClips.Count)].clip, ricochetVolume);
            else
                AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX,
                ricochetClips[(int)Random.Range(0, ricochetClips.Length)].clip, ricochetVolume);
        }
        else
        {
            if (surfaceSpecific)
                AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX,
                properClips[bounce%properClips.Count].clip, ricochetVolume);
            else
                AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX,
                ricochetClips[bounce % ricochetClips.Length].clip, ricochetVolume);
        }
    }
    public void PlayCollision()
    {
        AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX,
                collisionClips[(int)Random.Range(0, collisionClips.Length)].clip, throughSurfaceVolume);
    }
    public void PlayLunaBat()
    {

    }

}
