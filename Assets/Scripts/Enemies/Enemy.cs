using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Events;

[System.Serializable]
public struct StateKVP
{
    public string key;
    public StateData state;
}

public abstract class Enemy : MonoBehaviour, IShootable
{
    [Header("Additional State Data")]
    [Tooltip("Attach as needed")]
    public StateKVP[] stateDatas;
    public Dictionary<string, StateData> stateDataDict;

    [Header("Global")]
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject  itemPortrait;
    public Transform firePoint; // The position where the projectiles are spawned
    public float defaultMovementSpeed;
    public Animator animator;
    [HideInInspector] public EnemyStateMachine stateMachine;
    [HideInInspector] public EnemyIdleState idleState;

    [Header("Stunned Variables")]
    public GameObject lassoTarget;

    [Header("Death")]
    public UnityEvent onDeath;
    public bool standWhileDead;

    [Header("Loot")]
    public GameObject coinPrefab; // Reference to the coin prefab
    public int minCoins = 1; // Minimum number of coins to drop
    public int maxCoins = 5; // Maximum number of coins to drop
    public float coinDropRadius = 1.5f; // Radius within which coins will scatter

    protected NavMeshAgent nav;

    public delegate void DamageDelegate();
    public DamageDelegate damageDelegate;

    protected void MyAwake()
    {
        nav = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyStateMachine();
        //idleState.Initialize(this, stateMachine);
        LoadUpDictionary();
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
       StartCoroutine(DeathRoutine());

        // Spawn coins
        int coinsToDrop = Random.Range(minCoins, maxCoins + 1);
        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * coinDropRadius;
            spawnPosition.y = transform.position.y; // Keep coins spawning at the enemy's feet level
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }

        itemPortrait.SetActive(false);
    }

    private IEnumerator DeathRoutine()
    {
        stateMachine.ChangeState(idleState);

        //turn off components so it stops moving
        animator.enabled = false;
        nav.enabled = false;

        yield return new WaitForSeconds(0.5f);
        if(!standWhileDead) transform.DORotate(new Vector3(-88, 0, 0), 1.5f, RotateMode.WorldAxisAdd).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(2f);
        if (!standWhileDead) transform.DOMove(new Vector3(transform.position.x, transform.position.y -10, transform.position.z), 1f);

        //destroy this object
        if (!standWhileDead) Destroy(gameObject, 3f);

        onDeath?.Invoke();

        //destroy this script
        if (!standWhileDead) Destroy(this);
    }

    public void TakeDamage(int delta)
    {
        currentHealth -= delta;
        damageDelegate?.Invoke();
        if(currentHealth < 0)
        {
            Die();
        }

    }

    public StateData FindData(string name)
    {
        stateDataDict.TryGetValue(name, out var data);
        if (data == null) Debug.LogWarning("Custom Enemy Null Data! The state data for " + name + " was not found in the Enemy's dictionary");
        return data;
    }

    private void LoadUpDictionary()
    {
        stateDataDict = new();
        foreach (var dataPiece in stateDatas)
            stateDataDict.Add(dataPiece.key, dataPiece.state);
    }

    public virtual void OnShot(BulletController bullet)
    {
        TakeDamage((int)bullet.currDmg);
    }
}
