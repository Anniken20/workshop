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

    public HorseChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void EnterState()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        //this.transform.LookAt(player);
        readyingCharge = true;
        this.transform.LookAt(player);
        base.EnterState();
        nav.speed = 45;
        StartCoroutine(ReadyingCharge());

        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate(){
        base.PhysicsUpdate();
        if(readyingCharge && !activelyCharging){
            this.transform.LookAt(player);
        }
    }

    public override void ExitState()
    {  
        base.ExitState();
        nav.speed = enemy.DefaultMovementSpeed; // Reset the speed to the default value
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        activelyCharging = false;
    }

    private IEnumerator ReadyingCharge(){
        yield return new WaitForSeconds(2f);
        readyingCharge = false;
        nav.SetDestination(transform.position + this.transform.forward * 15);
    }
}