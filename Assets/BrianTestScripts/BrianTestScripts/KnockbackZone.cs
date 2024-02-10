using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class KnockbackZone : MonoBehaviour 
{
   public float knockbackStrength = 10f;
    public float knockbackCooldown = 2f; // Cooldown in seconds
    private float lastKnockbackTime = -Mathf.Infinity;

    public Vector3 pushDirection = new Vector3(0, 0, 1);

    public float initialDelay = 1f; // Initial delay in seconds before applying knockback

    private void OnCollisionEnter(Collision col)
    {
        // Call the HitEffect function when a collision occurs
        HitEffect(col);
    }

    public void HitEffect(Collision col)
    {
        if (Time.time - lastKnockbackTime < knockbackCooldown) return; // Cooldown not elapsed

        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DelayedKnockback(col));
        }
    }

    private IEnumerator DelayedKnockback(Collision col)
    {
        yield return new WaitForSeconds(initialDelay); // Wait for the specified delay

        
        if (Time.time - lastKnockbackTime < knockbackCooldown) yield break;

        ThirdPersonController controller = col.gameObject.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            Vector3 direction = pushDirection.normalized;
            controller.Push(direction * knockbackStrength);

            lastKnockbackTime = Time.time; // Update last knockback time
        }
    }
}