using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesmanMazeWall : MonoBehaviour, IShootable
{
    public Salesman salesmanRef;
    public void OnShot(BulletController bullet)
    {
        if(salesmanRef.canDoStuff == true){
            salesmanRef.BulletState();
            var Pos = bullet.gameObject.transform;
            salesmanRef.gameObject.GetComponent<SalesmanBulletState>().HeadToTarget(Pos, false);
        }
    }
}
