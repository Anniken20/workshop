using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiScriptVersion2 : MonoBehaviour
{
    public Transform player; // Reference to the player GameObject
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public List<Transform> coverPoints; // List of cover positions
    private int currentCoverIndex = 0;
    public Transform[] waypoints; // Array of patrol waypoints
    private int currentWaypointIndex = 0;

    // Define enum for different AI states
    private enum AIState
    {
        Idle,
        Patrolling,
        Chase,
        TakeCover,
        Attack,
    }
    private AIState currentState;
    private float attackRange = 2.0f; // Attack range
    private float chaseRange = 10.0f; // Chase Range
    private float visionRange = 15.0f; // Viosion Range
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = AIState.Patrolling;

        if (waypoints.Length > 0)
        {
            // Set the initial destination to the first waypoint
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Check if the player is within the vision range to transition to the Chase state.
                if (Vector3.Distance(transform.position, player.position) < visionRange)
                {
                    currentState = AIState.Chase;
                }
                break;

            case AIState.Patrolling:
                // Check if the enemy has reached the current waypoint
                if (navMeshAgent.remainingDistance < 0.1f)
                {
                    // Move to the next waypoint
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
                }
                break;

            case AIState.Chase:
                // Chase the player
                navMeshAgent.SetDestination(player.position);

                 // Check if the player is out of the chase range to transition back to Idle.
                if (Vector3.Distance(transform.position, player.position) > chaseRange)
                {
                    currentState = AIState.Idle;
                }

                if (Vector3.Distance(transform.position, player.position) < attackRange)
                {
                    // Transition to Attack state when in range
                    currentState = AIState.Attack;
                }
                break;
                
            case AIState.Attack:
                // Execute the attack behavior
                if (Vector3.Distance(transform.position, player.position) > attackRange)
                {
                    // Transition back to Chase state when the player is out of range
                    currentState = AIState.Chase;
                }
                //INSERT ALL ATTACK LOGIC HERE AND DAMAGE TO PLAYER
                break;

            case AIState.TakeCover:
                if (currentCoverIndex < coverPoints.Count)
                {
                    // Move to the next cover point
                    navMeshAgent.SetDestination(coverPoints[currentCoverIndex].position);

                    if (Vector3.Distance(transform.position, coverPoints[currentCoverIndex].position) < 2f)
                    {
                        // Transition to Chase state when in cover
                        currentState = AIState.Chase;
                        currentCoverIndex++;
                        if (currentCoverIndex >= coverPoints.Count)
                            currentCoverIndex = 0;
                    }
                }
                else
                {
                    // No more cover points, return to Chase state
                    currentState = AIState.Chase;
                }
                break;
        }
    }
}
