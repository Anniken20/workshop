using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesmanDamage : MonoBehaviour
{
    private Salesman s;
    private SalesmanData salesData;
    [SerializeField] int damage;
    [SerializeField] float damageCD;
    private bool canTrigger = true;
    private void Start()
    {
        s = GetComponentInParent<Salesman>();
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canTrigger)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && canTrigger)
            {
                playerHealth.TakeDamage(damage);
                StartCoroutine(DamageTimer());
                
            }
            canTrigger = false;
        }
    }
    private IEnumerator DamageTimer()
    {
        yield return new WaitForSeconds(damageCD);
        canTrigger = true;
    }
}

