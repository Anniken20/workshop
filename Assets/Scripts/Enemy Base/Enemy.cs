using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set;}
    public Rigidbody RB { get; set; }
    public bool IsFacingRight { get; set; } = true;
    private void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody>();
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if(CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        //TO DO Fill once statemachine is created
    }

    public enum AnimationTriggerType;
    {
        EnemyDamaged,
        PlayFootStepSound
    }

    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if(IsFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }

        else if (!IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }

    public void Die()
    {

    }
}
