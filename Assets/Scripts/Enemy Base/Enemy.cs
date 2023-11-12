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

    [Header("AOEAttack Variables")]
    public GameObject aoeAttackPrefab; // The AOE attack effect prefab
    public Transform aoeAttackPoint; // The position where the AOE attack is centered
    public float aoeRadius = 5.0f; // The radius of the AOE effect
    public float attackCooldown = 5.0f; // Cooldown between AOE attacks

    public float DefaultMovementSpeed;

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
