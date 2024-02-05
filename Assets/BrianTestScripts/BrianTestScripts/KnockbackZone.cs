using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class KnockbackZone : OnPlayerHit
{
    public float knockbackStrength = 10f;
    public float knockbackCooldown = 2f; // Cooldown in seconds
    private float lastKnockbackTime = -Mathf.Infinity;

    public Vector3 pushDirection = new Vector3(0, 0, 1);

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
            ThirdPersonController controller = col.gameObject.GetComponent<ThirdPersonController>();
            if (controller != null)
            {
                // Use the predefined pushDirection instead of calculating it from the collision
                Vector3 direction = pushDirection.normalized;
                controller.Push(direction * knockbackStrength);

                lastKnockbackTime = Time.time; // Update last knockback time
            }
        }
    }
}

