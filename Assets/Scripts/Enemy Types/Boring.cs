using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boring : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyAttackState attackState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyAOEAttackState enemyAOEAttackState;

    [Header("Pacing Variables")]
    public Vector2 frequencyBounds = new Vector2(2f, 6f);
    public float randomPointRadius = 5f;

    private void Awake()
    {
        base.MyAwake();


        /*
         * 
         * Unfortunately we can't use constructors for MonoBehaviours.
         * BUT We can instantiate them using AddComponent<>();
         * And then we can pass in their necessary references using the Initialize function, 
         * which I setup in the EnemyState script. 
         * 
         * 
        idleState = new EnemyIdleState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        pacingState = new EnemyPacingState(this, stateMachine);
        enemyAOEAttackState = new EnemyAOEAttackState(this, stateMachine);
        */
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
        attackState = gameObject.AddComponent<EnemyAttackState>();
        attackState.Initialize(this, stateMachine);
        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        //additional function call here to pass in data to the aoe attack
        //(since we can't set up its fields in the Inspector)
        enemyAOEAttackState = gameObject.AddComponent<EnemyAOEAttackState>();
        enemyAOEAttackState.Initialize(this, stateMachine);
        enemyAOEAttackState.SetupAttackData(aoeAttackPrefab, aoeAttackPoint, aoeRadius, attackCooldown);


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

    public void EnterAOEState()
    {
        if (currentHealth < 10) return;

        stateMachine.ChangeState(enemyAOEAttackState);
    }
}

