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

    public HorseChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void EnterState()
    {
        canLook = true;
        readyingCharge = true;
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        //this.transform.LookAt(player);
        //this.transform.LookAt(player);
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
        //Debug.Log("Currently current pos: " +this.transform.position +" Destination pos: " +destination);
        /*if(this.transform.position == nav.destination){
            Debug.Log("Destination Reached");
        }*/

        //if(nav.velocity == Vector3.zero){
            /*var chargeTrigger = GetComponentInParent<HorseChargeTrigger>();
            chargeTrigger.canTrigger = true;*/
        //}

    }

    public override void ExitState()
    {  
        base.ExitState();
        nav.speed = enemy.defaultMovementSpeed; // Reset the speed to the default value
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        activelyCharging = false;
    }

    private IEnumerator ReadyingCharge(){
        yield return new WaitForSeconds(1f);
        canLook = false;
        readyingCharge = false;
        destination = (transform.position + this.transform.forward * 15);
        nav.SetDestination(destination);
        StartCoroutine(LookDelay());
        
    }
    private IEnumerator LookDelay(){
        yield return new WaitForSeconds(1f);
        canLook = true;
    }
}