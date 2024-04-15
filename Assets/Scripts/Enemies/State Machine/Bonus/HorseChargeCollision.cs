using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseChargeCollision : MonoBehaviour
{
    private Horse h;
    public ChargeData chargeData;

    void Start(){
        h = GetComponentInParent<Horse>();
    }
    void OnTriggerEnter(Collider other){
        if (h == null) return;
        if(h.isCharging){
            if (other.gameObject.CompareTag("Player")){
                LaunchPlayer(other);
                
            }
            else if(other.gameObject.tag != "Lasso" && other.gameObject.tag != "Floor"){
                h.Stunned();
                Debug.Log("Hit: " +other.gameObject.name);
            }
        }
    }
    private void LaunchPlayer(Collider other){
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(chargeData.chargeDamage);
            Debug.Log("Damaging player for: " + chargeData.chargeDamage);

            
        }
    }
}
