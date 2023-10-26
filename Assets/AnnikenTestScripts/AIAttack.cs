using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttack : MonoBehaviour
{
    private string currentState = "Idle";

    public void SetState(string newState)
    {
        currentState = newState;
    }

    public void Update()
    {
        switch (currentState)
        {
            case "Idle":
                // Perform actions for the "Idle" state
                SetState("Searching");
                break;

            case "Searching":
                // Perform actions for the "Searching" state
                if (EnemyDetected())
                {
                    SetState("Engaging");
                }
                break;

            case "Engaging":
                // Perform actions for the "Engaging" state
                if (ShouldLaunch())
                {
                    SetState("Launching");
                }
                else if (ShouldAoE())
                {
                    SetState("AoEAttack");
                }
                else if (ShouldLob())
                {
                    SetState("Lobbing");
                }
                else if (ShouldMelee())
                {
                    SetState("MeleeAttacking");
                }
                else if (ShouldSlam())
                {
                    SetState("Slamming");
                }
                else if (ShouldShot())
                {
                    SetState("Shooting");
                }
                break;

            case "Launching":
                // Transition to a new state when the action is complete
                SetState("Idle");
                break;

            // Implement similar blocks for other states (e.g., AoEAttack, Lobbing, MeleeAttacking, Slamming, Shooting)
        }
    }
    private bool EnemyDetected()
    {
        // Implement logic to detect enemies
        return true; 
    }

    private bool ShouldLaunch()
    {
        // Implement logic for deciding whether to launch
        return true; 
    }

    private bool ShouldAoE()
    {
        // Implement logic for deciding whether to use AoE attack
        return false;
    }

    private bool ShouldLob()
    {
        // Implement logic for deciding whether to lob
        return false; 
    }

    private bool ShouldMelee()
    {
        // Implement logic for deciding whether to melee attack
        return false; 
    }

    private bool ShouldSlam()
    {
        // Implement logic for deciding whether to slam
        return false; 
    }

    private bool ShouldShot()
    {
        // Implement logic for deciding whether to shoot
        return false; 
    }
}