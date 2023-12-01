using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class GhostController : MonoBehaviour
{   
    public CharacterMovement iaControls;
    private InputAction phase;
    public Transform player; // Reference to the player's Transform
    public Transform box; // Reference to the box's Transform
    public float teleportDistance = 1f;

    [HideInInspector] public bool inGhost = false;
    private bool abilityEnabled = false;
    private float abilityDuration = 5.0f;
    private float countdownTimer = 5.0f;
    private bool playerInBox;
    private Vector3 originalPosition;
    //public ParticleSystem smokeParticleSystem; // 

    public Image abilityDurationBar;
    //can add the smoke to her hands if we want to, might need tweaking and editing but easy fix
    //The timer for the countdown need to be the same as the ability and match the material switch or it will bug out
    
 
    //Once per frame
    void Update()
    {  

        if (phase.triggered)
        {
            ToggleAbility();

            if (abilityEnabled)
            {
                abilityDurationBar.fillAmount = 1.0f; //Sets fill amount to 100% initially
            }
        }
        
            if (abilityEnabled)
                {
                    countdownTimer -= Time.deltaTime;

                    // Update UI bar based on remaining duration
                    abilityDurationBar.fillAmount = countdownTimer / abilityDuration;


                    if (countdownTimer <= 0)
                    {
                        DisableAbility();
                        GetComponent<MaterialSwitch>().ToggleMaterial();
                    }
                }
    }

    void ToggleAbility()
    {
        GetComponent<MaterialSwitch>().ToggleMaterial();
        abilityEnabled = !abilityEnabled;
        
        if (abilityEnabled)
        {
            countdownTimer = abilityDuration;

            EnableAbility();
        }
        else
        {
            DisableAbility();
        }
    }

    void EnableAbility()
    {
       // smokeParticleSystem.Play();
        GetComponent<BoxCollider> ().isTrigger = true;
        inGhost = true;
        originalPosition = player.position;
    }

    void DisableAbility()
    {
        //smokeParticleSystem.Stop();
        GetComponent<BoxCollider> ().isTrigger = false;
        inGhost = false;
        
        // Teleport the player to the valid position
        if(playerInBox){
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.position = originalPosition;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            playerInBox = false;
        }
        abilityEnabled = false;

        abilityDurationBar.fillAmount = 1f;

    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            playerInBox = true;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            playerInBox = false;
        }
    }
    private void Awake(){
        iaControls = new CharacterMovement();
    }
    private void OnEnable(){
        phase = iaControls.CharacterControls.Phase;

        phase.Enable();
    }
    private void OnDisable(){
        phase.Disable();
    }
}
