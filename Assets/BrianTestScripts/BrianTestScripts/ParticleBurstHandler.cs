using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBurstHandler : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public int burstCount = 10; // Adjust the number of particles emitted per burst.

    public void EmitBurst()
    {
        if (particleSystem != null)
        {
            particleSystem.Emit(burstCount);
        }
    }
}
