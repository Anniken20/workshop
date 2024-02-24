using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesmanMazeWall : MonoBehaviour, IShootable
{
    private Salesman salesmanRef;
    //private bool checkDist;
    private void Start()
    {
        salesmanRef = FindObjectOfType<Salesman>();
    }
    public void OnShot(BulletController bullet)
    {
        salesmanRef.BulletState();
        var Pos = bullet.gameObject.transform;
        salesmanRef.gameObject.GetComponent<SalesmanBulletState>().HeadToTarget(Pos, false);
    }
}
