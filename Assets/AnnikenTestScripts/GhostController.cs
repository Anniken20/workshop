using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour
{   
    private bool abilityEnabled = false;
    private float abilityDuration = 5.0f;
    private float countdownTimer = 2.0f;

    //public ParticleSystem smokeParticleSystem; // 

    //can add the smoke to her hands if we want to, might need tweaking and editing but easy fix
    //The timer for the countdown need to be the same as the ability and match the material switch or it will bug out
    
    //Once per frame
    void Update()
    {  
        if (Input.GetKeyDown (KeyCode.T))
        {
            ToggleAbility();
        }
            if (abilityEnabled)
                {
                    countdownTimer -= Time.deltaTime;

                    if (countdownTimer <= 0)
                    {
                        DisableAbility();
                    }
                }
        
    }

    void ToggleAbility()
    {
        abilityEnabled = !abilityEnabled;
        
        if (abilityEnabled)
        {
            countdownTimer = abilityDuration;

            EnableAbility();
        }
        else
        {
            DisableAbility();
        }
    }
    void EnableAbility()
    {
       // smokeParticleSystem.Play();
        GetComponent<BoxCollider> ().isTrigger = true;
        Debug.Log("ACTIVE");
    }

    void DisableAbility()
    {
       // smokeParticleSystem.Stop();
        GetComponent<BoxCollider> ().isTrigger = false;
        Debug.Log("DISABLED");
    }
}
