using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class KnockbackZone : OnPlayerHit
{
    public float knockbackStrength = 10f;
    public float knockbackCooldown = 2f; // Cooldown in seconds
    private float lastKnockbackTime = -Mathf.Infinity;

    public override void HitEffect(Collision col)
    {
        if (Time.time - lastKnockbackTime < knockbackCooldown) return; // Cooldown not elapsed

        base.HitEffect(col);

        if (col.gameObject.CompareTag("Player"))
        {
            ThirdPersonController controller = col.gameObject.GetComponent<ThirdPersonController>();
            if (controller != null)
            {
                Vector3 direction = col.transform.position - transform.position;
                direction.y = 0; // Keep the force horizontal
                controller.Push(direction.normalized * knockbackStrength);

                lastKnockbackTime = Time.time; // Update last knockback time
            }
        }
    }
}
