using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SocialiteMoveState : EnemyState
{
    private SocialiteMoveData moveData;

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
    }
    public override void ExitState()
    {
        base.ExitState();
        if (enemy.animator != null) enemy.animator.SetBool("Running", false);
    }
    private void SetDestination()
    {
        var moveDist = Random.Range(moveData.minMoveDist, moveData.maxMoveDist);
        var destination = (transform.position + GetDirection() * moveDist);
        //Debug.Log($"{destination} {moveDist}");
      //this.transform.LookAt(destination);
        nav.SetDestination(destination);
        StartCoroutine(MoveCooldown());
    }
    Vector3 GetDirection()
    {
        int randomDirection = Random.Range(0, 4);
        return moveData.directions[randomDirection];
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
