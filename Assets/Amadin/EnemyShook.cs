using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class EnemyShook : MonoBehaviour
{
    public Slider mainHealthSlider; // Reference to the main health slider
    public Slider laggingHealthSlider; // Reference to the lagging health slider
    public GameObject enemyPortrait;
    public float lagTime = 2f; // Time in seconds for the lagging health bar to catch up
    public float shakeDuration = 0.5f;
    public float shakeStrength = 0.1f;
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;
    public float lagFadeDuration = 1f; // Duration for the lagging health bar to fade out after boss takes damage

    private List<Enemy> enemies = new List<Enemy>();
    private bool bossTakingDamage = false; // Flag to track if boss is currently taking damage

    private void OnEnable()
    {
        // Find all enemies in the scene and add them to the list
        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        enemies.AddRange(enemyArray);

        // Subscribe to the delegate for each enemy
        foreach (Enemy enemy in enemies)
        {
            enemy.damageDelegate += OnBossDamaged;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the delegate for each enemy
        foreach (Enemy enemy in enemies)
        {
            enemy.damageDelegate -= OnBossDamaged;
        }

        // Clear the list of enemies
        enemies.Clear();
    }

    private void OnBossDamaged()
    {
        // Set flag to indicate boss is taking damage
        bossTakingDamage = true;

        // Shake the portrait
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
        // Update main health slider
        float totalHealth = 0f;
        float totalMaxHealth = 0f;
        foreach (Enemy enemy in enemies)
        {
            totalHealth += enemy.currentHealth;
            totalMaxHealth += enemy.maxHealth;
        }
        float newHealth = totalHealth / totalMaxHealth;
        mainHealthSlider.value = newHealth;

        if (bossTakingDamage)
        {
            // Calculate lagging health slider's target value based on main health slider's value
            float lagTargetValue = mainHealthSlider.value;

            // Update lagging health slider with a delay
            DOTween.To(() => laggingHealthSlider.value, x => laggingHealthSlider.value = x, lagTargetValue, lagTime)
                .OnComplete(() =>
                {
                    // After lag time, fade out the lagging health bar
                    FadeOutLaggingHealthBar();
                });
        }
    }

    private void FadeOutLaggingHealthBar()
    {
        // Fade out the lagging health slider's fill image 
        Image laggingFillImage = laggingHealthSlider.fillRect.GetComponent<Image>();
        laggingFillImage.DOFade(0f, lagFadeDuration).OnComplete(() =>
        {
            // Reset flag and set lagging health slider's value to match main health slider's value
            bossTakingDamage = false;
            laggingHealthSlider.value = mainHealthSlider.value;

            // Reset alpha back to 1
            laggingFillImage.DOFade(1f, 0f);
        });
    }
}