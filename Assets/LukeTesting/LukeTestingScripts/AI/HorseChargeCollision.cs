using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseChargeCollision : MonoBehaviour
{
    private Horse h;

    void Start(){
        h = GetComponentInParent<Horse>();
    }
    void OnTriggerEnter(Collider other){
        if(h.isCharging){
            if (other.gameObject.CompareTag("Player")){
                LaunchPlayer(other);
                
            }
            else{
                //h.Stunned();
            }
        }
    }
    private void LaunchPlayer(Collider other){
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(h.chargeDamage);
            Debug.Log("Damaging player for: " +h.chargeDamage);

            
        }
    }
}
