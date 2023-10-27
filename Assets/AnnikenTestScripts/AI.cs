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
    private float rangeAttackDistance = 10.0f;
    private Transform target;
    public Animator animator;
    private float attackRange = 2.0f; // Attack range
    private float chaseRange = 10.0f; // Chase Range
    private float visionRange = 15.0f; // Viosion Range

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
                break;

            case AIState.MeleeAttack:
                break;

            case AIState.RangeAttack:
                break;

            case AIState.ChargeAttack:
                break;

            case AIState.Evade:
                break;

            case AIState.TakeCover:
                break;

            case AIState.SlamAttack:
                break;

            case AIState.Hide:
                break;

            case AIState.LaunchAttack:
                break;

            case AIState.AoEAttack:
                break;

            case AIState.LobAttack:
                break;

            case AIState.Die:
                break;

            case AIState.Walk:
                break;

        }
    }
}