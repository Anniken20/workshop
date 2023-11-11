/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShootState : EnemyState
{
    public GameObject projectilePrefab; // The projectile or bullet prefab to be instantiated
    public Transform firePoint; // The position where the projectiles are spawned
    public float projectileSpeed = 10.0f; // The speed of the projectile
    public float fireRate = 1.0f; // Rate of fire (shots per second)
    private float nextFireTime; // Time for the next shot

    public EnemyShootState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        nextFireTime = Time.time;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (CanShoot())
        {
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
        GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Set the projectile's initial velocity
        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        // Update the next fire time based on the fire rate
        nextFireTime = Time.time + 1 / fireRate;
    }
}
*/