using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
    if (currentHealth <= 0)
    {
        return;
    }

    currentHealth -= damage;

    if (currentHealth <= 0)
    {
        Die();
    }
    }
        void Die()
        {
            //Add animation?
        }
}
