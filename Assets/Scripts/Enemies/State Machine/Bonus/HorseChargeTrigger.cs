using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseChargeTrigger : MonoBehaviour
{
    private Horse h;
    private HorseStunnedState sState;
    [HideInInspector] public bool canTrigger = true;
    [SerializeField] private GameObject stunnedText;
    //private NavMeshAgent horseNav;
    [HideInInspector] bool horseStunned;
    public float stunnedDuration;
    private bool startStunnedCountdown;
    private float internalStunDuration;
    private bool resetStunDuration;
    [HideInInspector] public bool wrangling;

    void Start(){
        h = GetComponentInParent<Horse>();
        sState = GetComponentInParent<HorseStunnedState>();
    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player") && canTrigger && sState.isStunned == false){
            //h.Freeze();
            h.Charge();
            canTrigger = false;
            StopCoroutine(TriggerDelay());
        }

    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            //h.StopCharge();
            //canTrigger = true;
            StartCoroutine(TriggerDelay());
        }
    }
    private void Update(){
        horseStunned = h.isStunned;
    }
    private void FixedUpdate(){
        /*var rb = this.GetComponentInParent<Rigidbody>();
        if(rb.velocity == Vector3.zero && !canTrigger){
            Debug.Log("Trigger charging: " +rb.velocity);
            h.Charge();
        }*/
        //Debug.Log(canTrigger);
        if (!wrangling)
        {
            if (horseStunned == true)
            {
                stunnedText.SetActive(true);
                startStunnedCountdown = true;
                if (resetStunDuration)
                {
                    internalStunDuration = stunnedDuration;
                    resetStunDuration = false;
                }
            }
            else
            {
                stunnedText.SetActive(false);
                startStunnedCountdown = false;
                resetStunDuration = true;
            }
        }
        if(startStunnedCountdown && !wrangling){
            internalStunDuration -= Time.deltaTime;
            if(internalStunDuration <= 0){
                h.Charge();
                horseStunned = false;
                startStunnedCountdown = false;
                h.isStunned = false;
            }
        }
    }
    private IEnumerator TriggerDelay(){
        yield return new WaitForSeconds(5.5f);
        canTrigger = true;
    }
}
