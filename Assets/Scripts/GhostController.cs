using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour
{   
    private bool abilityEnabled = false;
    private float abilityDuration = 5.0f;
    private float countdownTimer = 0.0f;
    
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
        GetComponent<BoxCollider> ().isTrigger = true;
        Debug.Log("ACTIVE");
    }

    void DisableAbility()
    {
        GetComponent<BoxCollider> ().isTrigger = false;
        Debug.Log("DISABLED");
    }
}
