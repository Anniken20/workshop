using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour
{
    public Transform playerTransform; // Player's Transform
    public float detectionRadius = 5f; // Detection radius for the player
    public float interactionDistance = 3f; // Distance to stop and interact
    private NavMeshAgent agent; // NavMeshAgent component
    public Transform startPoint; // Starting point of the NPC
    public Transform endPoint; // End point of the NPC
    private Vector3 currentTarget; // Current navigation target
    public Vector3 postInteractionDirection = Vector3.forward; // Default facing direction after interaction
    private bool isInteractingWithPlayer = false; // Interaction flag
    public AudioSource ambientSound; // Ambient sound source
    public Animator anim; // Animator for the NPC

    public enum MovementAxis { X, Z }
    public MovementAxis movementAxis = MovementAxis.X; // Default movement axis

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!agent) Debug.LogWarning("No NavMeshAgent attached to " + gameObject.name);
        
        // Calculate and cache world positions for start and end points
        currentTarget = startPoint.position;
        if (agent.isOnNavMesh) 
            agent.SetDestination(currentTarget);
        
        anim.SetBool("Idle", false);
        anim.SetBool("Walking", true);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Detect and handle player interaction
        if (distanceToPlayer <= interactionDistance)
        {
            if (!isInteractingWithPlayer)
            {
                agent.isStopped = true;
                agent.ResetPath();
                anim.SetBool("Walking", false);
                anim.SetBool("Idle", true);
                isInteractingWithPlayer = true;
            }
            FaceTarget(playerTransform.position); // Face the player
        }
        else
        {
            if (isInteractingWithPlayer)
            {
                // Reset interaction and face default direction
                isInteractingWithPlayer = false;
                FaceDirection(postInteractionDirection);
                agent.isStopped = false;
                agent.SetDestination(currentTarget);
                anim.SetBool("Idle", false);
                anim.SetBool("Walking", true);
            }

            // Check if it's time to switch targets
            if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                SwitchCurrentTarget();
            }
        }

        // Handle ambient sound based on interaction state
        HandleAmbientSound(isInteractingWithPlayer);
    }

    void SwitchCurrentTarget()
    {
        currentTarget = currentTarget == startPoint.position ? endPoint.position : startPoint.position;
        agent.SetDestination(currentTarget);
    }

    void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        FaceDirection(direction);
    }

    void FaceDirection(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void HandleAmbientSound(bool isInteracting)
    {
        if (ambientSound)
        {
            if (!isInteracting && !ambientSound.isPlaying)
            {
                ambientSound.Play();
            }
            else if (isInteracting && ambientSound.isPlaying)
            {
                ambientSound.Stop();
            }
        }
    }
}