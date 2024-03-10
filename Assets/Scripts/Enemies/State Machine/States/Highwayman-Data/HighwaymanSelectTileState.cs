using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class HighwaymanSelectTileState : EnemyState
{
    private Transform player;
    private GameObject[] tiles;
    public GameObject tile1;
    public GameObject tile2;
    public GameObject preferedTile;
    private HighwaymanData highwayData;
    public GameObject belowTile;
    public GameObject previousDestination;
    public HighwaymanSelectTileState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }
    public override void EnterState(){
        base.EnterState();
        highwayData = (HighwaymanData)enemy.FindData("HighwayData");
        
        tiles = this.GetComponent<Highwayman>().tiles.tileObjs;
        FindClosestTiles();
        //Debug.Log("Prefered Tile: " +preferedTile);
    }
    public override void ExitState(){
        base.ExitState();
    }
    public override void PhysicsUpdate(){
        base.PhysicsUpdate();
        player = ThirdPersonController.Main.gameObject.transform;
    }
    private void FindClosestTiles(){
        float minDistance1 = Mathf.Infinity;
        float minDistance2 = Mathf.Infinity;
        foreach(GameObject tile in tiles){
            if(tile != belowTile && tile != previousDestination){
                if(/*player != null*/tile.GetComponent<HighwaymanFloor>().isAvailable){
                    float distance = Vector3.Distance(transform.position, tile.transform.position);
                    if(distance< minDistance1){
                        minDistance2 = minDistance1;
                        tile2 = tile1;
                        minDistance1 = distance;
                        tile1 = tile;
                    }
                    else if(distance < minDistance2){
                        minDistance2 = distance;
                        tile2 = tile;
                    }
                    //Debug.Log("Tile: " +tile.name +"Distance: " +distance);
                }
                else{
                    //Debug.Log("Selected tile '" +tile.name +"' is not available");
                }
            }
            //Debug.Log("Closest Two Tiles: " +tile1 +" & " +tile2);
        }
        CompareToPlayer();
    }
    private void CompareToPlayer(){
        if(tile1 != null && tile2 != null && player != null){
            float tile1Dist = Vector3.Distance(tile1.transform.position, player.position);
            float tile2Dist = Vector3.Distance(tile2.transform.position, player.position);
            //Debug.Log("Stored POS:" +player.position +"Actual POS: " +FindObjectOfType<LassoController>().transform.position);
            //Debug.Log("Tile1 Dist: " +tile1Dist);
            //Debug.Log("Tile2 Dist: " +tile2Dist);
            if(tile1Dist < tile2Dist){
                preferedTile = tile1;
                //Debug.Log("Going with tile 1");
                this.GetComponent<HighwaymanTeleportState>().TargetTile = tile1;
                this.GetComponent<Highwayman>().Teleport();
                previousDestination = tile1;
            }
            else{
                preferedTile = tile2;
                //Debug.Log("Going with tile 2");
                this.GetComponent<HighwaymanTeleportState>().TargetTile = tile2;
                this.GetComponent<Highwayman>().Teleport();
                previousDestination = tile2;
            }
        }
        else{
            Debug.LogWarning("Could not select a valid tile");
        }
    }
    private void OnTriggerEnter(Collider other){
        belowTile = other.gameObject;
    }
}
