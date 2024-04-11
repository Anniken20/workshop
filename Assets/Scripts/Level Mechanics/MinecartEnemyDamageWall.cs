using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinecartEnemyDamageWall : MonoBehaviour
{
    public int damage;
    public UnityEvent onEnemyHit;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Enemy>() != null)
        {
            Debug.Log("HIT ENEMY RIDER FOR " + damage + " DAMAGE");
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            onEnemyHit?.Invoke();
        }
    }
}
