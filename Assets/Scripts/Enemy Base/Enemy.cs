using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public EnemyStateMachine stateMachine;
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyAttackState attackState;

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();

        idleState = new EnemyIdleState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);

    }
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public enum AnimationTriggerType;
    {
        EnemyDamaged,
        PlayFootStepSound
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
