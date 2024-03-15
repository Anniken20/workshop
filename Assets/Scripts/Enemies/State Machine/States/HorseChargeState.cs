using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorseChargeState : EnemyState
{
    private Transform player; // Reference to the player's transform
    public float maxChargeDist = 15; //Max distance the horse will charge to
    public float chargeSpeed = 5.0f; // Speed at which the enemy charges
    private Vector3 targetPos;
    private bool readyingCharge;
    private bool activelyCharging;
    private Vector3 destination;
    private bool canLook = true;
    private Animator anim;

    public HorseChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void EnterState()
    {
        //if(enemy.animator != null) enemy.animator.SetBool("Running", true);
        anim = this.GetComponent<Animator>();
        canLook = true;
        readyingCharge = true;
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        base.EnterState();
        nav.speed = 50;
        nav.acceleration = 20;
        StartCoroutine(ReadyingCharge());

        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate(){
        base.PhysicsUpdate();
        if(readyingCharge && canLook){
            this.transform.LookAt(player);
        }
        var distanceFromTarget = Vector3.Distance(this.gameObject.transform.position, nav.destination);
            if (distanceFromTarget <= 1.25 && GetComponentInChildren<HorseChargeTrigger>().wrangling == false)
            {
                if (enemy.animator != null) enemy.animator.SetBool("Idle", true);
                if(enemy.animator != null) enemy.animator.SetBool("Running", false);
                anim.SetBool("Running", false);
                EnterState();
            }

    }

    public override void ExitState()
    {  
        base.ExitState();
        nav.speed = enemy.defaultMovementSpeed; // Reset the speed to the default value
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        activelyCharging = false;
        if(enemy.animator != null) enemy.animator.SetBool("Running", false);
        anim.SetBool("Running", false);
    }

    private IEnumerator ReadyingCharge(){
        yield return new WaitForSeconds(1f);
        canLook = false;
        readyingCharge = false;
        destination = (transform.position + this.transform.forward * 15);
        nav.SetDestination(destination);
        if (enemy.animator != null) enemy.animator.SetBool("Idle", false);
        anim.SetBool("Idle", false);
        if (enemy.animator != null) enemy.animator.SetBool("Running", true);
        anim.SetBool("Running", true);
        StartCoroutine(LookDelay());
        
    }
    private IEnumerator LookDelay(){
        yield return new WaitForSeconds(1f);
        canLook = true;
        if(enemy.animator != null) enemy.animator.SetBool("Running", false);
        anim.SetBool("Running", false);
        if (enemy.animator != null) enemy.animator.SetBool("Idle", true);
        anim.SetBool("Idle", true);
    }
}