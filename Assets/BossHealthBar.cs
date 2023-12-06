using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    enum Boss{
        Sheriff,
        Santana,
        Diana
    }
    public float currentHealth;
    [SerializeField] Boss boss = new Boss();

    void Update(){
        if(boss.ToString() == "Sheriff"){
            var Sheriff = this.GetComponent<Sheriff>();
            float maxHealth = Sheriff.maxHealth;
            currentHealth = Sheriff.currentHealth;
            float healthPercentage = (currentHealth / maxHealth);
            healthSlider.value = healthPercentage;
            
        }
        else if(boss.ToString() == "Santana"){
            var Santana = this.GetComponent<Santana>();
            float maxHealth = Santana.maxHealth;
            currentHealth = Santana.currentHealth;
            float healthPercentage = (currentHealth / maxHealth);
            healthSlider.value = healthPercentage;
            
        }
        else if(boss.ToString() == "Diana"){
            var Diana = this.GetComponent<Diana>();
            float maxHealth = Diana.maxHealth;
            currentHealth = Diana.currentHealth;
            float healthPercentage = (currentHealth / maxHealth);
            healthSlider.value = healthPercentage;
            
        }
    }
}
