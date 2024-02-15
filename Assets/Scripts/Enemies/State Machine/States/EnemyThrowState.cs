using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class EnemyThrowState : EnemyState
{
    private Coroutine throwRoutine;
    private ThrowData throwData;
    public EnemyThrowState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        throwData = (ThrowData)enemy.FindData("throwData");
        Debug.Log("Started throw attack");
        throwRoutine = StartCoroutine(ThrowRoutine());
    }

    public override void ExitState()
    {
        base.ExitState();
        StopCoroutine(throwRoutine);
        Debug.Log("Stopped throw attack");
    }

    private IEnumerator ThrowRoutine()
    {
        while (true)
        {
            // wait windup time
            yield return new WaitForSeconds(throwData.windupTime);

            // create and throw the projectile at target
            Vector3 direction = (ThirdPersonController.Main.CorePosition() - enemy.firePoint.position).normalized;
            GameObject projectile = Instantiate(throwData.throwThing, enemy.firePoint.position, Quaternion.identity);
            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj != null) proj.Project(direction, throwData.throwAirSpeed);

            yield return new WaitForSeconds(throwData.timeBetweenThrows);
        }
    }
}
