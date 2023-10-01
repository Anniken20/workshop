using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour
{   
    public Transform player; // Reference to the player's Transform
    public Transform box; // Reference to the box's Transform
    public float teleportDistance = 1f;

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
        
        // Find a valid position on the ground outside the box
        Vector3 randomPosition = GetValidPositionOutsideBox();

        // Teleport the player to the valid position
        player.position = randomPosition;
    }

    private Vector3 GetValidPositionOutsideBox()
    {
        Vector3 boxCenter = box.position;
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0; // Ensure no vertical displacement

        Vector3 randomPosition = boxCenter + randomDirection.normalized * teleportDistance;

        // Ensure that the random position is not inside the box
        while (IsPointInsideBox(randomPosition))
        {
            randomDirection = Random.onUnitSphere;
            randomDirection.y = 0;
            randomPosition = boxCenter + randomDirection.normalized * teleportDistance;
        }

        // Cast a ray from the random position downwards to find the ground
        RaycastHit hit;
        if (Physics.Raycast(randomPosition + Vector3.up * 100f, Vector3.down, out hit, 200f, LayerMask.GetMask("Ground")))
        {
            randomPosition = hit.point;
        }

        return randomPosition;
    }

    private bool IsPointInsideBox(Vector3 point)
    {
        Vector3 boxCenter = box.position;
        Vector3 boxSize = box.localScale;

        // Check if the point is inside the box using the box's size
        return Mathf.Abs(point.x - boxCenter.x) < boxSize.x / 2f
            && Mathf.Abs(point.z - boxCenter.z) < boxSize.z / 2f;
    }
}
