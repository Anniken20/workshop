using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Vector3 initialPlayerPosition;

    private bool playerInBox;

    void Start()
    {
        // Record the initial player position
        initialPlayerPosition = player.position;
    }

    void Update()
    {  
        if (phase.triggered)
        {
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
        // Activate the ability
        inGhost = true;
        Debug.Log("ACTIVE");
    }

    void DisableAbility()
    {
        // Deactivate the ability
        inGhost = false;
        Debug.Log("DISABLED");
        
        // Teleport the player back to the initial position
        TeleportToInitialPosition();
        abilityEnabled = !abilityEnabled;
    }

    void TeleportToInitialPosition()
    {
        player.gameObject.GetComponent<CharacterController>().enabled = false;
        player.position = initialPlayerPosition;
        player.gameObject.GetComponent<CharacterController>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInBox = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInBox = false;
        }
    }

    private void Awake()
    {
        iaControls = new CharacterMovement();
    }

    private void OnEnable()
    {
        phase = iaControls.CharacterControls.Phase;
        phase.Enable();
    }

    private void OnDisable()
    {
        phase.Disable();
    }
}
