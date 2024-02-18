using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialiteMist : MonoBehaviour
{
    [SerializeField] private float mistDuration;
    [SerializeField] private int mistDamage;
    [SerializeField] private float mistDamageCD;
    private bool canDamage = true;
    private void Awake()
    {
        Destroy(this.gameObject, mistDuration);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && canDamage)
            {
                playerHealth.TakeDamage(mistDamage);
                canDamage = false;
                StartCoroutine(DamageCD());
            }
        }
    }
    private IEnumerator DamageCD()
    {
        yield return new WaitForSeconds(mistDamageCD);
        canDamage = true;
    }
}
