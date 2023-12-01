using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthBarImage; 
    public DeathScreenManager deathScreenManager;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        
    }

    void UpdateHealthUI()
    {
    if (healthBarImage != null)
        {
            float fillAmount = (float)currentHealth / maxHealth; // Calculate fill amount based on health
            healthBarImage.fillAmount = fillAmount; // Update the fill amount of the image
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
        Debug.Log("Player health: " + currentHealth);

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
