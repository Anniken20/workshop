using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
public class Boring : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyAttackState attackState;
    [HideInInspector] public EnemyPacingState pacingState;

    [Header("Pacing Variables")]
    public Vector2 frequencyBounds = new Vector2(2f, 6f);
    public float randomPointRadius = 5f;

    private void Awake()
    {
        base.MyAwake();
        idleState = new EnemyIdleState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        pacingState = new EnemyPacingState(this, stateMachine);

        //set default state
        stateMachine.Initialize(pacingState);
    }

    public void InIdleRange()
    {
        stateMachine.ChangeState(idleState);
    }

    public void BeginPacing()
    {
        stateMachine.ChangeState(pacingState);
    }
}
*/
