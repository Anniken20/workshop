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
    private Vector3 startPointWorldPosition;
    private Vector3 endPointWorldPosition;

    public enum MovementAxis
    {
    X,
    Z
    }

    public MovementAxis movementAxis = MovementAxis.X; // Default to X axis

   void Start()
   {
        agent = GetComponent<NavMeshAgent>();
        // Store the world positions of the start and end points
        startPointWorldPosition = startPoint.position;
        endPointWorldPosition = endPoint.position;
    
        // Set the initial target to the world position of the start point
        currentTarget = startPointWorldPosition;
        agent.SetDestination(currentTarget);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

       
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

        // Movement and target switching logic with world positions
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (currentTarget == startPointWorldPosition)
            {
                currentTarget = endPointWorldPosition;
            }
            else
            {
                currentTarget = startPointWorldPosition;
            }

            agent.SetDestination(currentTarget);
        }
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
