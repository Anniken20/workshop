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

    [Header("Shoot Variables")]
    public GameObject projectilePrefab; // The projectile or bullet prefab to be instantiated
    public Transform firePoint; // The position where the projectiles are spawned
    public float projectileSpeed = 10.0f; // The speed of the projectile
    public float fireRate = 1.0f; // Rate of fire (shots per second)

    [Header("Pacing Variables")]
    public Vector2 frequencyBounds = new Vector2(2f, 6f);
    public float randomPointRadius = 5f;

    [Header("Hiding Variables")]
    public float hidingDistance = 10.0f; // Minimum distance at which the enemy considers hiding
    public float hidingSpeed = 5.0f; // Speed at which the enemy moves to a hiding spot

    [Header("Launch Variables")]
    public GameObject launchProjectilePrefab; // The projectile to be launched
    public Transform launchPoint; // The position where the projectiles are launched from
    public float launchForce = 10.0f; // The force with which the projectile is launched

    [Header("Lob Variables")]
    public GameObject lobProjectilePrefab; // The projectile to be lobbed
    public Transform lobTarget; // The target where the lobbed projectile will land
    public float lobSpeed = 10.0f; // Speed of the lobbed projectile

    [Header("Melee Variables")]
    public float attackRange = 2.0f; // Range at which the enemy can perform a melee attack
    public int attackDamage = 10; // Damage dealt by the melee attack
    public float attackMeleeCooldown = 1.0f; // Cooldown between melee attacks

    [Header("Charge Variables")]
    public float chargeSpeed = 5.0f; // Speed at which the enemy charges

    [Header("Evade Variables")]
    public float evadeDistance = 5.0f; // Distance at which the enemy starts evading
    public float evadeSpeed = 5.0f; // Speed at which the enemy evades

    public float DefaultMovementSpeed;
    protected NavMeshAgent nav;

    protected void MyAwake()
    {
        nav = GetComponent<NavMeshAgent>();
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
