using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class EnemyHealthDisplay : MonoBehaviour
{
    public Image mainHealthBar;
    public Image laggingHealthBar;
    public GameObject enemyPortrait;
    public float lagTime = 2f; // Time in seconds for the lagging health bar to catch up
    public float shakeDuration = 0.5f;
    public float shakeStrength = 0.1f;
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;

    private List<Enemy> enemies = new List<Enemy>();

    private void OnEnable()
    {
        // Find all enemies in the scene and add them to the list
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        enemies.AddRange(enemyArray);

        // Subscribe to the delegate for each enemy
        foreach (Enemy enemy in enemies)
        {
            enemy.damageDelegate += ShakePortraitAndUpdateHealth;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the delegate for each enemy
        foreach (Enemy enemy in enemies)
        {
            enemy.damageDelegate -= ShakePortraitAndUpdateHealth;
        }

        // Clear the list of enemies
        enemies.Clear();
    }

    private void ShakePortraitAndUpdateHealth()
    {
        // Shake the enemy's portrait
        ShakePortrait();

        // Update health bars
        UpdateHealthBars();
    }

    private void ShakePortrait()
    {
        // Shake the portrait using DOTween
        enemyPortrait.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
    }

    private void UpdateHealthBars()
    {
        // Update main health bar
        float totalHealth = 0f;
        float totalMaxHealth = 0f;
        foreach (Enemy enemy in enemies)
        {
            totalHealth += enemy.currentHealth;
            totalMaxHealth += enemy.maxHealth;
        }
        float newHealth = totalHealth / totalMaxHealth;
        mainHealthBar.fillAmount = newHealth;

        // Calculate lagging health bar's target fill amount
        float lagTargetFill = Mathf.Clamp(newHealth, 0f, 1f);

        // Update lagging health bar with a delay
        DOTween.To(() => laggingHealthBar.fillAmount, x => laggingHealthBar.fillAmount = x, lagTargetFill, lagTime);
    }
}

