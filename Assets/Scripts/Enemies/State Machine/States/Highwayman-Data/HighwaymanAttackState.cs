using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class HighwaymanAttackState : EnemyState
{
    private HighwaymanData highwayData;
    public HighwaymanAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine){}
    public override void EnterState(){
        base.EnterState();
        highwayData = (HighwaymanData)enemy.FindData("HighwayData");
        PlayerHealth playerHealth = ThirdPersonController.Main.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            {
                playerHealth.TakeDamage(highwayData.damage);
            }
    }
    public override void ExitState(){
        base.ExitState();
    }
}
