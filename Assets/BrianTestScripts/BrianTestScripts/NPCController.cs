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
    private Vector3 wanderTarget = Vector3.zero;
    public Transform startPoint;
    public Transform endPoint;
    private Vector3 currentTarget;

    public enum MovementAxis
    {
    X,
    Z
    }

    public MovementAxis movementAxis = MovementAxis.X; // Default to X axis

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = startPoint.position;
        agent.SetDestination(currentTarget);
        StartCoroutine(UpdateDestinationRoutine());
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
    
        // Handle player interaction
        if (distanceToPlayer <= interactionDistance)
        {
        if (!agent.isStopped)
        {
            agent.isStopped = true;
            agent.ResetPath(); // Stop moving towards the current target
        }
        FaceTarget(playerTransform.position);
        }
        else
        {
        if (agent.isStopped || !agent.hasPath)
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget);
        }
        
        // Check if the AI has reached its current target
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Switch target
            if (currentTarget == startPoint.position)
            {
                currentTarget = endPoint.position;
            }
            else
            {
                currentTarget = startPoint.position;
            }
            
            agent.SetDestination(currentTarget);
        }
        }
    }

    IEnumerator UpdateDestinationRoutine()
    {
    while (true)
    {
        if (!agent.isStopped)
        {
            SetNewRandomDestination();
        }
        yield return new WaitForSeconds(30); // Wait for 30 seconds before updating the destination again
    }
    }

    void SetNewRandomDestination()
    {
        Vector3 currentPos = transform.position;
    
        // Depending on the chosen axis, move in a straight line along that axis
        if (movementAxis == MovementAxis.X)
        {
        wanderTarget = new Vector3(currentPos.x + Random.Range(-detectionRadius, detectionRadius), currentPos.y, currentPos.z);
        }
        else // MovementAxis.Z
        {
        wanderTarget = new Vector3(currentPos.x, currentPos.y, currentPos.z + Random.Range(-detectionRadius, detectionRadius));
        }

        agent.SetDestination(wanderTarget);
    }

    Vector3 RandomWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * detectionRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, detectionRadius, -1);
        return navHit.position;
    }

    void FaceTarget(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
