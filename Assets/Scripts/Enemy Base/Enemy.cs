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

    protected void MyAwake()
    {
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyStateMachine();
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState?.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState?.PhysicsUpdate();
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
*/