using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour
{
    public Transform playerTransform; // Assign the player's transform in the inspector
    public float detectionRadius = 5f; // Radius within which the AI can detect the player
    public float interactionDistance = 3f; // Distance within which the AI stops and faces the player
    private NavMeshAgent agent;
    public Transform startPoint;
    public Transform endPoint;
    private Vector3 currentTarget;
    private Vector3 startPointWorldPosition;
    private Vector3 endPointWorldPosition;
    public Vector3 postInteractionDirection = Vector3.forward; // Direction the NPC faces after interaction
    private bool isInteractingWithPlayer = false;
    public AudioSource ambientSound;
    public Animator anim;

    public enum MovementAxis
    {
        X,
        Z
    }

    public MovementAxis movementAxis = MovementAxis.X; // Default to X axis

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPointWorldPosition = startPoint.position;
        endPointWorldPosition = endPoint.position;
    
        currentTarget = startPointWorldPosition;
        agent.SetDestination(currentTarget);
        anim.SetBool("Idle", false);
        anim.SetBool("Walking", true);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

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
            FaceTarget(playerTransform.position); // Face the player during interaction
        }
        else
        {
            if (isInteractingWithPlayer)
            {
                // Once player exits the interaction range, NPC faces the predetermined direction
                isInteractingWithPlayer = false;
                FaceDirection(postInteractionDirection);
                agent.isStopped = false;
                agent.SetDestination(currentTarget);
                anim.SetBool("Idle", false);
                anim.SetBool("Walking", true);
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                SwitchCurrentTarget();
            }
        }

        HandleAmbientSound(isInteractingWithPlayer);
    }

    void SwitchCurrentTarget()
    {
        currentTarget = currentTarget == startPointWorldPosition ? endPointWorldPosition : startPointWorldPosition;
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
        if (ambientSound != null)
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