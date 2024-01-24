using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
    private float abilityDuration = 10.0f;
    private float countdownTimer = 10.0f;
    private bool playerInBox;
    
    [Header("Time Stats")]
    [SerializeField] private float _phasingDisplayTime = 5.0f;
    [SerializeField] private float _phasingFadeOutTime = 0.5f;

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _fullScreenPhasing;
    [SerializeField] private Material _material;

    [Header("Intensity Stats")]
    [SerializeField] private float _voronoIntensityStat = 2.5f;
    [SerializeField] private float _vignetteIntensityStat = 1.25f;

    private int _voronoIntensity = Shader.PropertyToID("_VoronoIntensity");
    private int _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

    public AudioSource src;
    public AudioClip enterAudio;
    public AudioClip duringAudio;
    public AudioClip exitAudio;
    private Vector3 originalPosition;
    //public ParticleSystem smokeParticleSystem; // 
    public Image abilityDurationBar;
    //can add the smoke to her hands if we want to, might need tweaking and editing but easy fix
    //The timer for the countdown need to be the same as the ability and match the material switch or it will bug out
    
    //public GameObject popUpImage;
 
    //Once per frame
    void Update()
    {
        if (phase.triggered)
        {
            ToggleAbility();
            StartCoroutine(Phasing());
        }

        if (abilityEnabled)
        {
            countdownTimer -= Time.deltaTime;

            // Update UI bar based on remaining duration
            abilityDurationBar.fillAmount = Mathf.Clamp01(countdownTimer / abilityDuration);
            
             /* Enable pop-up image when ability is active
            if (!popUpImage.activeSelf)
            {
                popUpImage.SetActive(true);
            }*/

            if (countdownTimer <= 0)
            {
                DisableAbility();
                GetComponent<MaterialSwitch>().ToggleMaterial();

                /* Disable pop-up image when ability is not active
                if (popUpImage.activeSelf)
                {
                    popUpImage.SetActive(false);
                }*/
            }
        }
        else if (abilityDurationBar.fillAmount < 1.0f)
        {
            // Only recharge the ability bar when the ability is not active and the fill amount is not at maximum
            RechargeAbilityBar();

            /* Disable pop-up image when ability is not active
            if (popUpImage.activeSelf)
            {
                popUpImage.SetActive(false);
            }*/
        }
    }

    private IEnumerator Phasing()
    {
        _fullScreenPhasing.SetActive(true);
        _material.SetFloat(_voronoIntensity, _voronoIntensityStat);
        _material.SetFloat(_vignetteIntensity, _vignetteIntensityStat);

        yield return new WaitForSeconds(_phasingDisplayTime);

        float elapsedTime = 0f;
        while (elapsedTime < _phasingFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedVoronoi = Mathf.Lerp(_voronoIntensityStat, 0f, (elapsedTime / _phasingFadeOutTime));
            float lerpedVignette = Mathf.Lerp(_vignetteIntensityStat, 0f, (elapsedTime / _phasingFadeOutTime));

            _material.SetFloat(_voronoIntensity, lerpedVoronoi);
            _material.SetFloat(_vignetteIntensity, lerpedVignette);

            yield return null;
        }

        _fullScreenPhasing.SetActive(false);
    }

    void RechargeAbilityBar()
    {
        float rechargeRate = 0.05f; // Change Recharge speed
        abilityDurationBar.fillAmount += Time.deltaTime * rechargeRate;
    }

    void ToggleAbility()
    {
        GetComponent<MaterialSwitch>().ToggleMaterial();
        abilityEnabled = !abilityEnabled;

        if (abilityEnabled)
        {
            countdownTimer = abilityDuration * abilityDurationBar.fillAmount;
            // smokeParticleSystem.Play();
            GetComponent<BoxCollider> ().isTrigger = true;
            inGhost = true;
            originalPosition = player.position;

            /* Disable pop-up image when ability is not active
            if (popUpImage.activeSelf)
            {
                popUpImage.SetActive(true);
            }*/
        }
        if (abilityEnabled == false)
        {
            //smokeParticleSystem.Stop();
            GetComponent<BoxCollider> ().isTrigger = false;
            inGhost = false;

            /* Disable pop-up image when ability is not active
            if (popUpImage.activeSelf)
            {
                popUpImage.SetActive(false);
            }*/

            // Teleport the player to the valid position
        if(playerInBox){
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.position = originalPosition;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            playerInBox = false;
        }
        }
    }

    void EnableAbility()
    {
        src.PlayOneShot(enterAudio);
       // smokeParticleSystem.Play();
        GetComponent<BoxCollider> ().isTrigger = true;
        inGhost = true;
        originalPosition = player.position;
    }

    void DisableAbility()
    {
        src.PlayOneShot(exitAudio);
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

        abilityDurationBar.fillAmount = 0.0f;

    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            playerInBox = true;
            src.PlayOneShot(duringAudio);

        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            playerInBox = false;
            src.PlayOneShot(exitAudio);

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
