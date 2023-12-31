using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShootState : EnemyState
{
    private Transform player; // Reference to the player's transform
    private float nextFireTime; // Time for the next shot

    public EnemyShootState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextFireTime = Time.time;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (CanShoot())
        {
            FacePlayer();
            Shoot();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private bool CanShoot()
    {
        return Time.time >= nextFireTime;
    }

    private void Shoot()
    {
        // Create a projectile and set its position and rotation
        GameObject newProjectile = Instantiate(enemy.projectilePrefab, enemy.firePoint.position, enemy.firePoint.rotation);
        EnemyBullet bullet = newProjectile.GetComponent<EnemyBullet>();

        bullet.Initialize(transform.forward);


        // Update the next fire time based on the fire rate
        nextFireTime = Time.time + 1 / enemy.fireRate;
    }

    private void FacePlayer()
    {
        nav.updateRotation = false;
        Vector3 lookVector = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        transform.LookAt(lookVector);
    }
}
