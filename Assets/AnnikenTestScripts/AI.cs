using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
All AI State behaviours and their logic will need to be implemented here,
including death which I haven't implemented yet

10/26/23 Anniken
*/
public class AI : MonoBehaviour
{ 
    private Transform target;
    public Animator animator;
    public AIState CurrentState { get; private set; }

    public void SetState(AIState newState)
    {
        CurrentState = newState;
    }

    public void Update()
    {
        // Implement logic for each state
        switch (CurrentState)
        {
            case AIState.Idle:
                IdleBehaviour();
                break;

            case AIState.MeleeAttack:
                MeleeAttackBehaviour();
                break;

            case AIState.RangeAttack:
                RangeAttackBehaviour();
                break;

            case AIState.ChargeAttack:
                ChargeAttackBehavior();
                break;

            case AIState.Evade:
                EvadeBehavior();
                break;

            case AIState.TakeCover:
                TakeCoverBehavior();
                break;

            case AIState.SlamAttack:
                SlamAttackBehavior();
                break;

            case AIState.Hide:
                HideBehavior();
                break;

            case AIState.LaunchAttack:
                LaunchAttackBehavior();
                break;

            case AIState.AoEAttack:
                AoEAttackBehavior();
                break;

            case AIState.LobAttack:
                LobAttackBehavior();
                break;

            case AIState.Die:
                DieBehavior();
                break;

            case AIState.Walk:
                WalkBehavior();
                break;

        }
    }

    private void RangeAttackBehaviour() 
    {

    }

    private void MeleeAttackBehaviour() 
    {

    }

    private void IdleBehaviour()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("IdleTrigger"); // Assuming we have an "Idle" trigger in our Animator Controller
        }
        float idleDuration = 5.0f; // Adjust the duration as needed
        StartCoroutine(WaitAndTransition(idleDuration));
    }

    private IEnumerator WaitAndTransition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetState(AIState.Chase);
    }

    private void EvadeBehavior()
    {

    }

    private void ChargeAttackBehavior()
    {

    }

    private void HideBehavior()
    {

    }

    private void SlamAttackBehavior()
    {

    }

    private void LaunchAttackBehavior()
    {

    }

    private void AoEAttackBehavior()
    {

    }

    private void LobAttackBehavior()
    {

    }

    
    private void WalkBehavior()
    {

    }

    private void TakeCoverBehavior()
    {
        List<Vector3> coverPositions = new List<Vector3>();

        // Choose the nearest available cover position
        Vector3 nearestCoverPosition = FindNearestCover(coverPositions);

        if (nearestCoverPosition != Vector3.zero)
        {
            // Move towards the cover position
            MoveToCover(nearestCoverPosition);
        }
        else
        {
            SetState(AIState.Evade);
        }
    }   

    private Vector3 FindNearestCover(List<Vector3> coverPositions)
        {
        Vector3 closestCover = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (var coverPosition in coverPositions)
        {
            float distance = Vector3.Distance(transform.position, coverPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCover = coverPosition;
            }
        }
        return closestCover;
    }
    private void MoveToCover(Vector3 coverPosition)
        {
        float moveSpeed = 5f; // Adjust movement speed as needed

        Vector3 direction = (coverPosition - transform.position).normalized;
        Vector3 movement = direction * moveSpeed * Time.deltaTime;

        // Move the AI character
        transform.Translate(movement);
        }
    
    private void DieBehavior()
    {

    }

}