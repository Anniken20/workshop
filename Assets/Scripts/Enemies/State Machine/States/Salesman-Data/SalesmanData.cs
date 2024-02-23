using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/SalesmanData")]
public class SalesmanData : StateData
{
    public float maxFollowDistance;
    public float locationWaitTime;
    public float stopFollowingDistance;
    public float moveSpeed;
    public float acceleration;
}