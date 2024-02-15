using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/ShootData")]
public class ShootData : StateData
{
    public GameObject projectilePrefab;
    public float fireRate;
}
