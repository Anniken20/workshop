using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/SocialiteMoveData")]
public class SocialiteMoveData : StateData
{
    public float moveSpeed;
    public float moveCooldown;
    public float maxMoveDist;
    public float minMoveDist;
    public Vector3[] directions = new Vector3[4];
    public GameObject mistObj;
    public float mistSpawnCD;
}
