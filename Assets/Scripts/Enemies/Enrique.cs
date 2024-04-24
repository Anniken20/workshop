using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Events;

public class Enrique : Enemy, ICryable
{
    [HideInInspector] public CryState cryState;
    [HideInInspector] public RunToPointsState runToPointsState;
    private bool inBattle;
    public AudioClip crying;
    public Animator anim;
    private int i = 0;

    private bool amCrying = false;

    public GhostSpawner[] ghostSpawners;
    private void Awake()
    {
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        runToPointsState = gameObject.AddComponent<RunToPointsState>();
        runToPointsState.Initialize(this, stateMachine);

        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState?.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState?.PhysicsUpdate();
    }

    public void BeginBattle()
    {
        inBattle = true;
        stateMachine.ChangeState(runToPointsState);
        ghostSpawners[4].gameObject.SetActive(true);
        ghostSpawners[5].gameObject.SetActive(true);
        ghostSpawners[6].gameObject.SetActive(true);
    }

    public void StartCrying()
    {
        amCrying = true;
        audioSource.PlayOneShot(crying);
    }

    
    public override void OnShot(BulletController bullet)
    {
        if (!inBattle) return;
        //Debug.Log(stateMachine.CurrentEnemyState);
        if (amCrying)
        {
            TakeDamage((int)bullet.currDmg);
            stateMachine.ChangeState(runToPointsState);
            //audioSource.Stop();
            if(currentHealth > 0)
                SpawnGhosts();
        }
    }

    private void SpawnGhosts()
    {
        if(i<4)
        {
            ghostSpawners[i].gameObject.SetActive(true);
        }
        i++;
    }

    public override void Die()
    {
        base.Die();
    }

    public void StopCrying()
    {
        amCrying = false;
    }
}
