using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highwayman : Enemy
{
    [HideInInspector] public HighwaymanSelectTileState selectTileState;
    [HideInInspector] public HighwaymanTeleportState teleportState;
    [HideInInspector] public TileContainer tiles;
    private void Awake(){
        base.MyAwake();
        tiles = FindObjectOfType<TileContainer>().GetComponent<TileContainer>();
        selectTileState = gameObject.AddComponent<HighwaymanSelectTileState>();
        selectTileState.Initialize(this, stateMachine);
        teleportState = gameObject.AddComponent<HighwaymanTeleportState>();
        teleportState.Initialize(this, stateMachine);
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
        stateMachine.Initialize(idleState);
    }
    public void SelectTile(){
        stateMachine.ChangeState(selectTileState);
    }
    public void ExitSelectTile(){
        stateMachine.ChangeState(idleState);
    }
    public void Teleport(){
        stateMachine.ChangeState(teleportState);
    }
    public void ExitTeleport(){
        stateMachine.ChangeState(idleState);
    }
    public void Idle(){
        stateMachine.ChangeState(idleState);
    }
    public void EditIdle(){
        stateMachine.ChangeState(selectTileState);
    }
}
