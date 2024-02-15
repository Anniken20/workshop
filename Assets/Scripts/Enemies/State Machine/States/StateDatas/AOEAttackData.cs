using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/AOEAttackData")]
public class AOEAttackData : StateData
{
    public GameObject aoeAttackPrefab;
    public float attackCooldown;
}
