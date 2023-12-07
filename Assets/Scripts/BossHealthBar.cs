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
    [SerializeField] Boss boss = new Boss();

    void Update(){
        if(boss.ToString() == "Sheriff"){
            var Sheriff = this.GetComponent<Sheriff>();
            float maxHealth = Sheriff.maxHealth;
            var currentHealth = Sheriff.currentHealth;
            float healthPercentage = (currentHealth / maxHealth);
            healthSlider.value = healthPercentage;
            
        }
        else if(boss.ToString() == "Santana"){
            var Santana = this.GetComponent<Santana>();
            float maxHealth = Santana.maxHealth;
            var currentHealth = Santana.currentHealth;
            float healthPercentage = (currentHealth / maxHealth);
            healthSlider.value = healthPercentage;
            
        }
        else if(boss.ToString() == "Diana"){
            var Diana = this.GetComponent<Diana>();
            float maxHealth = Diana.maxHealth;
            var currentHealth = Diana.currentHealth;
            float healthPercentage = (currentHealth / maxHealth);
            healthSlider.value = healthPercentage;
            
        }
    }
}
