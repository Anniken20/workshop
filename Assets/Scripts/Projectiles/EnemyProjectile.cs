using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    public int damage;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        Break();
    }
}
