using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;

    private void Update(){
        Enemy enemy = GetComponent<Enemy>();
        if (enemy == null) return;
        float maxHealth = enemy.maxHealth;
        float currentHealth = enemy.currentHealth;
        float healthPercentage = (currentHealth / maxHealth);
        healthSlider.value = healthPercentage;
        healthSlider.gameObject.transform.LookAt(Camera.main.transform);
    }
}
