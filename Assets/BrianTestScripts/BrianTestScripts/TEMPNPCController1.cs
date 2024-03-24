using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This is a band aid script made by Holly for the temp NPCs so they can have the interaction capabilities without the movement

public class TEMPNPCController1 : MonoBehaviour
{
    public Transform playerTransform; // Assign the player's transform in the inspector
    public float detectionRadius = 7f; // Radius within which the AI can detect the player
    public float interactionDistance = 3f; // Distance within which the AI stops and faces the player

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= interactionDistance)
        {
            FaceTarget(playerTransform.position);
        }
    }

    void FaceTarget(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
