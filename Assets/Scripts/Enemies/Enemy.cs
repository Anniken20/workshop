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

public abstract class Enemy : MonoBehaviour, IShootable, IDataPersistence
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
    public AudioSource audioSource;
    public AudioClip[] hurtSFX;
    public AudioClip movingSFX;
    public bool duelEnemy = false;

    [HideInInspector] public EnemyStateMachine stateMachine;
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public DuelState duelState;

    [Header("Stunned Variables")]
    public GameObject lassoTarget;

    [Header("Death")]
    public UnityEvent silentDeath;
    public UnityEvent onDeath;
    public bool standWhileDead;
    [HideInInspector]
    public enum Enemies{
        Horse,
        Enrique,
        Grant,
        Poppy,
        Jordan,
        Benita,
        Highwayman,
        Wren,
        Rocco,
        Carillo,
        Diana,
        Santana,
        Jabroni
    }
    public Enemies enemy = new Enemies();
    public static List<Enemies> enemiesList = new List<Enemies>();
    [Header("Loot")]
    public GameObject coinPrefab; // Reference to the coin prefab
    public int minCoins = 1; // Minimum number of coins to drop
    public int maxCoins = 5; // Maximum number of coins to drop
    public float coinDropRadius = 1.5f; // Radius within which coins will scatter
    [Header("Regen")]
    public bool regen;
    [Tooltip("Amount of damage needed to not regen")]
    public int regenDMG;
    [Tooltip("Time to wait before regen")]
    public float regenTimer;

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
    public void StartDuel()
    {
        stateMachine.ChangeState(duelState);
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

    public virtual void Die()
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

        if(itemPortrait != null) itemPortrait.SetActive(false);
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
        //if (!standWhileDead) Destroy(gameObject, 3f);
        if(!enemiesList.Contains(enemy)){
            enemiesList.Add(this.enemy);
        }
        onDeath?.Invoke();

        //destroy this script
        if (!standWhileDead) Destroy(this);
    }

    public virtual void TakeDamage(int delta)
    {
        currentHealth -= delta;
        if (hurtSFX.Length > 0)
        {
            // Choose a random death sound
            AudioClip randomHurtSound = hurtSFX[Random.Range(0, hurtSFX.Length)];

            //Play random SFX
            audioSource.PlayOneShot(randomHurtSound);
        }
        damageDelegate?.Invoke();
        if(regen && delta < regenDMG){
            StartCoroutine(Regenerate(delta));
        }
        if(currentHealth <= 0 && !duelEnemy)
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

    public void GoToIdle()
    {
        stateMachine.ChangeState(idleState);
    }

    public void Freeze()
    {
        //animator.playbackTime = 0f;
    }

    public void Unfreeze()
    {
        //animator.playbackTime = 1f;
    }
     public IEnumerator Regenerate(float dmg){
        yield return new WaitForSeconds(regenTimer);
        currentHealth += dmg;
    }
    public void LoadData(GameData data){
        if(data.deadEnemies.Contains(this.enemy)){
            SilentDeath();
        }
        enemiesList = data.deadEnemies;
    }
    public void SaveData(ref GameData data){
        data.deadEnemies = enemiesList;
    }
    private void SilentDeath(){
        silentDeath?.Invoke();
        Destroy(this.gameObject);
    }

}
