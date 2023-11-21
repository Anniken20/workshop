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
    private Vector3 randomPosition;

    private bool playerInBox;
    //public ParticleSystem smokeParticleSystem; // 

    //can add the smoke to her hands if we want to, might need tweaking and editing but easy fix
    //The timer for the countdown need to be the same as the ability and match the material switch or it will bug out
    
    //Once per frame
    void Update()
    {  
        if (phase.triggered)
        {
            randomPosition = player.position;
            ToggleAbility();
        }
        
            if (abilityEnabled)
                {
                    countdownTimer -= Time.deltaTime;

                    if (countdownTimer <= 0)
                    {
                        DisableAbility();
                    }
                }
    }

    void ToggleAbility()
    {
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
        Debug.Log("ACTIVE");
    }

    void DisableAbility()
    {
       // smokeParticleSystem.Stop();
        GetComponent<BoxCollider> ().isTrigger = false;
        inGhost = false;
        Debug.Log("DISABLED");
        
        // Find a valid position on the ground outside the box
        //Vector3 randomPosition = GetValidPositionOutsideBox();

        // Teleport the player to the valid position
        if(playerInBox){
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            randomPosition.y = 0;
            player.position = randomPosition;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            playerInBox = false;
        }
        abilityEnabled = !abilityEnabled;

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
