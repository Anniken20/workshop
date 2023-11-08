using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Global")]
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public EnemyStateMachine stateMachine;
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyAttackState attackState;
    [HideInInspector] public EnemyPacingState pacingState;

    [Header("Pacing Variables")]
    public Vector2 frequencyBounds = new Vector2(2f, 6f);
    public float randomPointRadius = 5f;

    private void Awake()
    {
        NavMeshAgent nav = GetComponent<NavMeshAgent>();

        stateMachine = new EnemyStateMachine();

        idleState = new EnemyIdleState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        pacingState = new EnemyPacingState(this, stateMachine);

        stateMachine.Initialize(pacingState);
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootStepSound
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
