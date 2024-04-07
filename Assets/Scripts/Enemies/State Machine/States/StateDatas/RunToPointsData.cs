using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/RunToPointsData")]
public class RunToPointsData : StateData
{
    [Header("In World Space")]
    public Vector3[] points;
    [Tooltip("Enrique will stop moving when within this distance of his target point")]
    public float distanceTolerance;
    public bool cryAfterReachingDestination;
    public float maxTimeAtPoint;
}
