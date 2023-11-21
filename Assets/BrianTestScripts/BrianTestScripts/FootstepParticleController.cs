using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepParticleController : MonoBehaviour
{
public ParticleSystem leftFootParticles; // Reference to the particle system for the left foot
    public ParticleSystem rightFootParticles; // Reference to the particle system for the right foot

   [SerializeField]
    private Animator animator; // Reference to the character's animator

    bool isWalking = false; // Flag to track if the character is walking

    void Start()
    {
        if (animator == null)
        {
            // Get the Animator component if not set in the inspector
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Check if the character is in the "walking" state
        bool walkingState = animator.GetBool("IsWalking");

        // If the walking state changes, trigger footstep particles
        if (walkingState && !isWalking)
        {
            isWalking = true;
            EmitFootstepParticles();
        }
        else if (!walkingState && isWalking)
        {
            isWalking = false;
        }
    }

    void EmitFootstepParticles()
    {
        if (leftFootParticles != null && rightFootParticles != null)
        {
            // Emit particles for both feet
            leftFootParticles.Emit(10); 
            rightFootParticles.Emit(10); 
        }
    }
}
