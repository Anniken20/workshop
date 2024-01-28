using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseChargeTrigger : MonoBehaviour
{
    private Horse h;
    private bool canTrigger;

    void Start(){
        h = GetComponentInParent<Horse>();
    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player") && canTrigger){
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
