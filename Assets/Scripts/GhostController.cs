using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GhostController : MonoBehaviour
{   
    public CharacterMovement iaControls;
    private InputAction phase;
    public Transform player; 
    public Transform box; 
    public float teleportDistance = 1f;

    public AudioSource src;
    public AudioClip enterAudio;
    public AudioClip duringAudio;
    public AudioClip exitAudio;

    [HideInInspector] public bool inGhost = false;
    private bool abilityEnabled = false;
    private float abilityDuration = 5.0f;
    private float cooldownDuration = 10.0f;
    private float cooldownTimer = 0.0f;
    private float regenerationRate = 0.2f; 
    private bool playerInBox;
    private Vector3 originalPosition;

    public Image abilityDurationBar;
    
    void Update()
    {  
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            abilityDurationBar.fillAmount = cooldownTimer / cooldownDuration;
        }

        if (phase.triggered)
        {
            ToggleAbility();
        }

        if (abilityEnabled)
        {
            abilityDurationBar.fillAmount -= Time.deltaTime / abilityDuration;

            if (abilityDurationBar.fillAmount <= 0)
            {
                DisableAbility();
                GetComponent<MaterialSwitch>().ToggleMaterial();
                cooldownTimer = cooldownDuration;
            }
        }
        else if (cooldownTimer < cooldownDuration)
        {
            cooldownTimer = Mathf.Min(cooldownTimer + regenerationRate * Time.deltaTime, cooldownDuration);
            abilityDurationBar.fillAmount = cooldownTimer / cooldownDuration;
        }
    }

    void ToggleAbility()
    {
        if (!abilityEnabled && cooldownTimer <= 0)
        {
            GetComponent<MaterialSwitch>().ToggleMaterial();
            abilityEnabled = true;

            if (abilityDurationBar != null)
            {
                abilityDurationBar.fillAmount = 1f;
            }

            EnableAbility();
        }
        else if (abilityEnabled)
        {
            DisableAbility();
        }
    }

    void EnableAbility()
    {
        src.PlayOneShot(enterAudio);

        GetComponent<BoxCollider>().isTrigger = true;
        inGhost = true;
        originalPosition = player.position;
    }

    void DisableAbility()
    {
        src.PlayOneShot(exitAudio);

        GetComponent<BoxCollider>().isTrigger = false;
        inGhost = false;

        if (playerInBox)
        {
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.position = originalPosition;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            playerInBox = false;
        }

        abilityEnabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInBox = true;
            src.PlayOneShot(duringAudio);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInBox = false;
            src.PlayOneShot(exitAudio);
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