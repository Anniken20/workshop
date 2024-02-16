using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/LaunchData")]
public class LaunchData : StateData
{
    public GameObject launchProjectilePrefab;
    public float launchForce;
}
