using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class HighwaymanSelectTileState : EnemyState
{
    private Transform player;
    private GameObject[] tiles;
    private GameObject tile1;
    private GameObject tile2;
    private HighwaymanData highwayData;
    public HighwaymanSelectTileState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }
    public override void EnterState(){
        base.EnterState();
        highwayData = (HighwaymanData)enemy.FindData("HighwayData");
        player = ThirdPersonController.Main.gameObject.transform;
        tiles = this.GetComponent<Highwayman>().tiles.tileObjs;
        Debug.Log("Best tile to go to: " +FindClosestTiles());
    }
    public override void ExitState(){
        base.ExitState();
    }
    private GameObject FindClosestTiles(){
        float minDistance1 = float.MaxValue;
        float minDistance2 = float.MaxValue;
        foreach(GameObject tile in tiles){
            //Debug.Log(tile.name);
            if(tile.GetComponent<BrittleFloor>().isAvailable){
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
            }
        }
        Debug.Log("Closest Two Tiles: " +tile1 +" & " +tile2);
        return CompareToPlayer(tile1, tile2);
    }
    private GameObject CompareToPlayer(GameObject tile1, GameObject tile2){
        if(tile1 != null && tile2 != null && player != null){
            float tile1Dist = Vector3.Distance(tile1.transform.position, player.position);
            float tile2Dist = Vector3.Distance(tile2.transform.position, player.position);
            if(tile1Dist > tile2Dist){
                return tile1;
                Debug.Log("Optimal Tile: " +tile1);
            }
            else{
                return tile2;
                Debug.Log("Optimal Tile: " +tile2);
            }
        }
        else{
            return null;
            Debug.LogWarning("Could not select a valid tile");
        }
    }
}
