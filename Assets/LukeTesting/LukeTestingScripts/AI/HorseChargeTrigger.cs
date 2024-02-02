using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseChargeTrigger : MonoBehaviour
{
    private Horse h;
    private HorseStunnedState sState;
    private bool canTrigger;

    void Start(){
        h = GetComponentInParent<Horse>();
        sState = GetComponentInParent<HorseStunnedState>();
    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player") && canTrigger && sState.isStunned == false){
            h.Charge();
            canTrigger = false;
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            //h.StopCharge();
            canTrigger = true;
        }
    }
}
