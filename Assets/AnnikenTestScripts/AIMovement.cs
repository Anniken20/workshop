using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIMovementState
{
    Standing,
    Charging,
    Pacing,
    Evading
}

public class AIMovement : MonoBehaviour
{
    public float standDuration = 2.0f;
    public float chargeSpeed = 5.0f;
    public float paceSpeed = 2.0f;
    public float evadeSpeed = 3.0f;
    public Transform target;
    public float chargeStoppingDistance = 1.0f;

    private AIMovementState currentState = AIMovementState.Standing;
    private float stateTimer = 0.0f;
    private Vector3 targetPosition;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    
    public float paceRange = 5.0f;  // The range within which the AI will pace
    private Vector3 originalPosition;
    private float paceDirection = 1;

    void Start()
    {
        SetState(AIMovementState.Standing);
        originalPosition = transform.position;
    }

    void Update()
    {
        // Implement state machine logic
        switch (currentState)
        {
            case AIMovementState.Standing:
                StandBehavior();
                break;

            case AIMovementState.Charging:
                ChargeBehavior();
                break;

            case AIMovementState.Pacing:
                PaceBehavior();
                break;

            case AIMovementState.Evading:
                EvadeBehavior();
                break;
        }
    }

    void SetState(AIMovementState newState)
    {
        currentState = newState;
        stateTimer = 0.0f;

        // Handle state entry logic (if any)
        switch (newState)
        {
            case AIMovementState.Standing:
                // Perform actions for entering the Standing state
                break;

            case AIMovementState.Charging:
                // Perform actions for entering the Charging state
                break;

            case AIMovementState.Pacing:
                // Perform actions for entering the Pacing state
                break;

            case AIMovementState.Evading:
                // Perform actions for entering the Evading state
                break;
        }
    }

    void StandBehavior()
    {
        // Implement behavior for the Standing state
        stateTimer += Time.deltaTime;
        if (stateTimer >= standDuration)
        {
            SetState(AIMovementState.Charging);
        }
    }

    void ChargeBehavior()
    {
        if (target == null)
        {
            // If there's no target, transition to another state
            SetState(AIMovementState.Standing);
            return;
        }

        // Calculate the direction to the target
        Vector3 moveDirection = (target.position - transform.position).normalized;

        // Move the AI in the direction of the target
        transform.Translate(moveDirection * chargeSpeed * Time.deltaTime);

        // Check if the AI has reached the target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < chargeStoppingDistance)
        {
            // The AI has reached the target, transition to another state
            SetState(AIMovementState.Standing);
        }
    }
    void PaceBehavior()
    {
        // Calculate the position to pace to
        Vector3 paceTarget = originalPosition + Vector3.forward * paceDirection * paceRange;

        // Calculate the direction to the pacing target
        Vector3 moveDirection = (paceTarget - transform.position).normalized;

        // Move the AI in the direction of the pacing target
        transform.Translate(moveDirection * paceSpeed * Time.deltaTime);

        // Check if the AI has reached the pacing target
        float distanceToPaceTarget = Vector3.Distance(transform.position, paceTarget);

        if (distanceToPaceTarget < 0.1f) // You can adjust this threshold
        {
            // The AI has reached the pacing target, change direction
            paceDirection *= -1;
        }
    }

    void EvadeBehavior()
    {
        // Check if the target (e.g., the player) is in sight
        if (IsTargetInSight())
        {
            // Determine a direction away from the target
            Vector3 evadeDirection = (transform.position - targetPosition).normalized;

            // Move the AI away from the target
            transform.Translate(evadeDirection * evadeSpeed * Time.deltaTime);
        }
        else
        {
            // If the target is no longer in sight, transition to another state
            SetState(AIMovementState.Standing); // You can choose an appropriate state
        }

        // Implement additional logic for state transitions or obstacle avoidance
    }

    bool IsTargetInSight()
    {
        if (target == null)
        {
            // If there's no target, it's not in sight
            return false;
        }
        Vector3 directionToTarget = target.position - transform.position;
        RaycastHit hit;

        // Define a layer mask to control which objects the raycast can hit
        int layerMask = LayerMask.GetMask("ObstacleLayer"); // Adjust this to your obstacle layer

        // Perform the raycast
        if (Physics.Raycast(transform.position, directionToTarget, out hit, Mathf.Infinity, layerMask))
        {
            // Check if the hit object is the target
            if (hit.collider.transform == target)
            {
                // The target is in sight
                return true;
            }
        }
        // The target is not in sight, so you should return to pace here
        ReturnToPace(); // Implement your return to pace function

        // Return false to indicate that the target is not in sight
        return false;
    }

    void ReturnToPace()
    {
        UnityEngine.AI.NavMeshAgent navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (navAgent != null)
        {
            // Set the destination to a predefined "pace" position
            Vector3 pacePosition = new Vector3(0f, 0f, 0f); // Replace with your desired pace position
            navAgent.SetDestination(pacePosition);
        }
    }
}