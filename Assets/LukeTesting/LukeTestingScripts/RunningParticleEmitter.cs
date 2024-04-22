using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningParticleEmitter : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle1;
    [SerializeField] private ParticleSystem particle2;

    public void ToggleParticles(bool toggle){
        if (particle1 != null && particle2 != null){
            if(toggle){
                particle1.Play();
                particle2.Play();
            }
            else{
                particle1.Stop();
                particle2.Stop();
            }
        }
    }
}
