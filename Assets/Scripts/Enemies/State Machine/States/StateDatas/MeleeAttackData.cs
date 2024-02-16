using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/MeleeAttackData")]
public class MeleeAttackData : StateData
{
    public int attackDamage;
    public float attackCooldown;
    public float attackRange;
}
