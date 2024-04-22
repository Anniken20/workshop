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

    public GhostSpawner[] ghostSpawners;
    private void Awake()
    {
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        cryState = gameObject.AddComponent<CryState>();
        cryState.Initialize(this, stateMachine);

        runToPointsState = gameObject.AddComponent<RunToPointsState>();
        runToPointsState.Initialize(this, stateMachine);

        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(cryState);

        anim.SetBool("Idle", true);
        anim.SetBool("Crying", false);
        anim.SetBool("Running", false);
        anim.SetBool("Dead", false);
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
    }

    public void StartCrying()
    {
        stateMachine.ChangeState(cryState);
        audioSource.PlayOneShot(crying);
    }

    
    public override void OnShot(BulletController bullet)
    {
        if (!inBattle) return;
        if (stateMachine.CurrentEnemyState is CryState)
        {
            TakeDamage((int)bullet.currDmg);
            stateMachine.ChangeState(runToPointsState);
            //audioSource.Stop();
            SpawnGhosts();
        }
    }

    private void SpawnGhosts()
    {
        if(i<4)
        {
            ghostSpawners[i].gameObject.SetActive(true);
            if(i==0)
            {
                ghostSpawners[4].gameObject.SetActive(true);
                ghostSpawners[5].gameObject.SetActive(true);
                ghostSpawners[6].gameObject.SetActive(true);
            }
        }
        i++;
    }

    public override void Die()
    {
        base.Die();
    }
}
