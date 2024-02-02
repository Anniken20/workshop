using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PacingData")]
public class PacingData : StateData
{
    public float timeBetweenMovements;
    public float walkSpeed;
    public float maxDistToNewPoint;
    public float minDistToNewPoint;
}
