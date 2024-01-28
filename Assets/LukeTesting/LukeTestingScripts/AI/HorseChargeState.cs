using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorseChargeState : EnemyState
{
    private Transform player; // Reference to the player's transform
    public float maxChargeDist; //Max distance the horse will charge to
    private Transform startPos;
    public float chargeSpeed = 5.0f; // Speed at which the enemy charges

    public HorseChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    public override void EnterState()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        base.EnterState();
        Debug.Log("Petah the horse is charging");
        nav.speed = chargeSpeed;
        nav.SetDestination(player.position);
        startPos = this.transform;
        //this.GetComponent<Rigidbody>().AddForce(player.position* 5f);
        //Debug.Log(player.position);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue charging toward the player
        //nav.SetDestination(player.position);
        /*float distance = Vector3.Distance(startPos.position, transform.position);
        if(distance >= maxChargeDist){
            ExitState();
        }*/
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Petah the horse stopped charging");
        nav.speed = enemy.DefaultMovementSpeed; // Reset the speed to the default value
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}