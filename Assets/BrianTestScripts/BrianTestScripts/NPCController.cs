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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateDestinationRoutine());
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
    
    // Check if the player is within interaction distance
    if (distanceToPlayer <= interactionDistance)
    {
        // Stop and face the player
        if (!agent.isStopped) {
            agent.isStopped = true;
            agent.ResetPath(); // Clear current path to stop the movement
        }
        FaceTarget(playerTransform.position);
    }
    else if(agent.isStopped) // Add condition to restart movement when the player is not within interaction distance
    {
        agent.isStopped = false;
        // Optionally, immediately set a new destination when the player moves away
        SetNewRandomDestination();
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
    wanderTarget = RandomWanderTarget();
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
