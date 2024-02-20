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
        throwData = (ThrowData)enemy.FindData("ThrowData");
        throwRoutine = StartCoroutine(ThrowRoutine());
        if (enemy.animator != null) enemy.animator.SetBool("Throwing", true);
    }

    public override void ExitState()
    {
        if (enemy.animator != null) enemy.animator.SetBool("Throwing", false);
        StopCoroutine(throwRoutine);
        base.ExitState();
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
