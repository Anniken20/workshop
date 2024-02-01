using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Controller")]
    private InputAction phase;
    // public Transform box;
    //  public float teleportDistance = 1f;

    [HideInInspector] public bool inGhost = false;
    private bool abilityEnabled = false;
    private float abilityDuration = 5.0f;
    private float cooldownDuration = 5.0f; // Cooldown time for the ability in seconds
    private float toggleCooldown = 0.5f; // Cooldown time between toggles in seconds
    private float countdownTimer;
    //private bool playerInBox;
    //private Vector3 originalPosition;

    public Image abilityDurationBar;

    [Header("FullScreenTest Controller")]
    [SerializeField] private float _phasingDisplayTime = 5.0f;
    [SerializeField] private float _phasingFadeOutTime = 0.5f;
    [SerializeField] private ScriptableRendererFeature _fullScreenPhasing;
    [SerializeField] private Material _material;
    [SerializeField] private float _voronoIntensityStat = 2.5f;
    [SerializeField] private float _vignetteIntensityStat = 1.25f;

    private int _voronoIntensity = Shader.PropertyToID("_VoronoIntensity");
    private int _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

    private float maxBarValue = 1.0f; // Maximum value for the ability bar
    private float barRegenerationRate = 0.2f; // Rate at which the ability bar regenerates per second
    private bool isCooldownActive = false; // Flag to check if the ability is in cooldown
    private bool isToggleCooldownActive = false; // Flag to check if toggle cooldown is active

    private CharacterMovement iaControls;

    [Header("Audio")]
    public AudioClip enterAudioClip;
    public AudioClip duringAudioClip;
    public AudioClip exitAudioClip;
    public AudioSource audioSource;

    private bool hasPlayedEnterAudio = false;

    void Start()
    {
        iaControls = new CharacterMovement();
        iaControls.CharacterControls.Enable();
        phase = iaControls.CharacterControls.Phase;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Regenerate ability bar over time
        if (!abilityEnabled && !isCooldownActive && abilityDurationBar.fillAmount < maxBarValue)
        {
            abilityDurationBar.fillAmount += barRegenerationRate * Time.deltaTime;
        }

        // Ghost Controller functionality
        if (phase.triggered && !isCooldownActive && !isToggleCooldownActive)
        {
            ToggleAbility();
        }

        if (abilityEnabled)
        {
            countdownTimer -= Time.deltaTime;
            abilityDurationBar.fillAmount = countdownTimer / abilityDuration;

            if (countdownTimer <= 0)
            {
                DisableAbility();
                SwitchMaterial();
                StartCoroutine(AbilityCooldown());
            }
        }

        // FullScreenTest Controller functionality
        if (abilityEnabled)
        {
            _fullScreenPhasing.SetActive(true);
            _material.SetFloat(_voronoIntensity, _voronoIntensityStat);
            _material.SetFloat(_vignetteIntensity, _vignetteIntensityStat);
        }
        else
        {
            _fullScreenPhasing.SetActive(false);
        }
    }

    void ToggleAbility()
    {
        isToggleCooldownActive = true;
        StartCoroutine(ToggleCooldown());

        abilityEnabled = !abilityEnabled;

        if (abilityEnabled)
        {
            countdownTimer = abilityDuration;
            EnableAbility();
            
            // Play enter audio clip (if not played already)
            if (enterAudioClip != null && !hasPlayedEnterAudio)
            {
                audioSource.PlayOneShot(enterAudioClip);
                hasPlayedEnterAudio = true;
            }
        }
        else
        {
            DisableAbility();
            hasPlayedEnterAudio = false; // Reset the flag when exiting
            
            // Play exit audio clip
            if (exitAudioClip != null)
            {
                audioSource.PlayOneShot(exitAudioClip);
            }
        }
    }

    void EnableAbility()
    {
        // Ghost Controller functionality
        GetComponent<Collider>().isTrigger = true;
        inGhost = true;
        //originalPosition = transform.position;

        // FullScreenTest Controller functionality
        _fullScreenPhasing.SetActive(true);
        _material.SetFloat(_voronoIntensity, _voronoIntensityStat);
        _material.SetFloat(_vignetteIntensity, _vignetteIntensityStat);

        // Play during audio clip
        if (duringAudioClip != null)
        {
            audioSource.PlayOneShot(duringAudioClip);
        }
    }

    void DisableAbility()
    {
        // Ghost Controller functionality
        GetComponent<Collider>().isTrigger = false;
        inGhost = false;

        /*
        if (playerInBox)
        {
            var characterController = GetComponent<CharacterController>();
            characterController.enabled = false;
            transform.position = originalPosition;
            characterController.enabled = true;
            playerInBox = false;
        }
        */
        abilityEnabled = false;

        // FullScreenTest Controller functionality
        _fullScreenPhasing.SetActive(false);
    }

    IEnumerator Phasing()
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //playerInBox = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //playerInBox = false;
        }
    }

    public void SwitchMaterial()
    {
        Renderer[] allMats = GetComponentsInChildren<Renderer>();

        foreach (Renderer mat in allMats)
        {
            if (mat.material.shader != null)
            {
                mat.material.shader = abilityEnabled ? Shader.Find("Shader Graphs/Ghost Shader") : Shader.Find("Shader Graphs/LIT TOON");
            }
        }
    }

    IEnumerator AbilityCooldown()
    {
        isCooldownActive = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldownActive = false;
    }

    IEnumerator ToggleCooldown()
    {
        yield return new WaitForSeconds(toggleCooldown);
        isToggleCooldownActive = false;
    }

    private void OnDisable()
    {
        iaControls.CharacterControls.Disable();
    }
}
