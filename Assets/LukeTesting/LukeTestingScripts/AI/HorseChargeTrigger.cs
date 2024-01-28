using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseChargeTrigger : MonoBehaviour
{
    private Horse h;

    void Start(){
        h = GetComponentInParent<Horse>();
    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            h.Charge();
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            h.Pacing();
        }
    }
}
