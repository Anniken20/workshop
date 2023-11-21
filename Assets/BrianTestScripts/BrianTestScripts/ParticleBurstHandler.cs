using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBurstHandler : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float[] normalizedTimeToEmit; // Array of normalized times to emit particles

    private Animator animator;
    private bool[] hasEmitted; // To track if particles have been emitted at each time

    void Start()
    {
        animator = GetComponent<Animator>();
        hasEmitted = new bool[normalizedTimeToEmit.Length];
    }

    void Update()
    {
        float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        for (int i = 0; i < normalizedTimeToEmit.Length; i++)
        {
            if (!hasEmitted[i] && normalizedTime >= normalizedTimeToEmit[i])
            {
                if (particleSystem != null)
                {
                    Debug.Log("Emitting particles at normalized time: " + normalizedTimeToEmit[i]);
                    particleSystem.Emit(10); // Adjust particle count as needed
                    hasEmitted[i] = true;
                }
            }
        }
    }
}
