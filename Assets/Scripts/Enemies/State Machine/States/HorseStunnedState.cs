using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HorseStunnedState : EnemyState
{
    [HideInInspector] public bool isStunned;
    public GameObject lassoTarget;
    private Animator anim;
    public HorseStunnedState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        anim = this.GetComponent<Animator>();
        if(enemy.animator != null) enemy.animator.SetBool("Stunned", true);
        anim.SetBool("Running", false);
        anim.SetBool("Stunned", true);
        if (enemy.animator != null) enemy.animator.SetBool("Idle", false);
        anim.SetBool("Idle", false);
        isStunned = true;
        Enemy h = GetComponentInParent<Enemy>();
        lassoTarget = h.lassoTarget;
        nav.isStopped = true;
        lassoTarget.GetComponent<BoxCollider>().enabled = true;
        nav.SetDestination(this.transform.position);
    }

    public override void PhysicsUpdate(){
        base.PhysicsUpdate();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void ExitState()
    {
        if(enemy.animator != null) enemy.animator.SetBool("Stunned", false);
        anim.SetBool("Stunned", false);
        isStunned = false;
        nav.isStopped = false;
        lassoTarget.GetComponent<BoxCollider>().enabled = false;
        nav.SetDestination(this.transform.position);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

}
