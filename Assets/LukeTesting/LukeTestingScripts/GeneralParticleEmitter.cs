using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralParticleEmitter : MonoBehaviour
{
    public bool Testingtoggle;

    [SerializeField] ParticleSystem[] particles;
    private void FixedUpdate(){
        if(Testingtoggle){
            ToggleParticles(Testingtoggle);
            Testingtoggle = false;
        }
    }
    public void ToggleParticles(bool toggle){
        if(particles != null){
            if(toggle){
                foreach (ParticleSystem particle in particles){
                    particle.Play();
                }
            }
            else{
                foreach (ParticleSystem particle in particles){
                    particle.Stop();
                }
            }
        }
    }
    public void TurnOnParticles(){
        ToggleParticles(true);
    }
    public void TurnOffParticles(){
        ToggleParticles(false);
    }
}
