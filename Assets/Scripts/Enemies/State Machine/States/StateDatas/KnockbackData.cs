using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/KnockbackData")]
public class KnockbackData : StateData
{
    public float windupTime;
    public Vector3 destinationPosition;
    public GameObject knockbackPrefab;
    public float knockbackPower;
}
