using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwaymanTeleportState : EnemyState
{
    private HighwaymanData highwayData;
    [HideInInspector] public GameObject TargetTile;
    public HighwaymanTeleportState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }
    public override void EnterState(){
        base.EnterState();
        highwayData = (HighwaymanData)enemy.FindData("HighwayData");
        //this.transform.position = TargetTile.transform.position;
        this.transform.position = new Vector3(TargetTile.transform.position.x, 3.84f, TargetTile.transform.position.z);
    }
    public override void ExitState(){
        base.ExitState();
    }
}
