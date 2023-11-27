using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
public int maxHealth = 100;
    private int currentHealth;

    public Slider healthSlider; 
    public DeathScreenManager deathScreenManager;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth; 
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
        void Die()
        {
            if (deathScreenManager != null)
        {
            deathScreenManager.ShowDeathScreen();
        }
            //Add animation?
        }
}
