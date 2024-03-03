using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SocialiteMoveState : EnemyState
{
    private SocialiteMoveData moveData;
    private Vector3 point;
    public Vector3 lastDir = Vector3.zero;


    public SocialiteMoveState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        moveData = (SocialiteMoveData)enemy.FindData("MoveData");
        nav.speed = moveData.moveSpeed;
        SetDestination();
        StartCoroutine(SpawnMist());
        //Is this how animations are supposed to be set up? idk
        if (enemy.animator != null) enemy.animator.SetBool("Running", true);
 
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(point != null)
        {
            var distanceFromTarget = Vector3.Distance(this.gameObject.transform.position, nav.destination);
            if (distanceFromTarget <= 1.25)
            {
                SetDestination();
            }
        }
    }
    public override void ExitState()
    {
        base.ExitState();
        if (enemy.animator != null) enemy.animator.SetBool("Running", false);
    }
    private void SetDestination()
    {
        point = GetDirection();
        nav.SetDestination(point);
    }
    Vector3 GetDirection()
    {
        Vector3 farthestPoint = transform.position;
        float maxDistance = 0f;
        foreach (Vector3 position in moveData.directions)
        {
            if (lastDir != position)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, position, out hit))
                {
                    if (hit.distance > maxDistance)
                    {
                        maxDistance = hit.distance;
                        farthestPoint = hit.point;
                    }
                }
            }
        }
        lastDir = (farthestPoint - this.transform.position).normalized * -1;
        return farthestPoint;
    }
    private IEnumerator MoveCooldown()
    {
        yield return new WaitForSeconds(moveData.moveCooldown);
        SetDestination();
    }
    private IEnumerator SpawnMist()
    {
        var mistSpawn = transform.Find("MistSpawnPOS");
        Instantiate(moveData.mistObj, mistSpawn.position, Quaternion.identity);
        yield return new WaitForSeconds(moveData.mistSpawnCD);
        StartCoroutine(SpawnMist());

    }
}
