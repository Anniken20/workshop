using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/PacingData")]
public class PacingData : StateData
{
    public Vector2 frequencyBounds;
    public float randomPointRadius;
}
