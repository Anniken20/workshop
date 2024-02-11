using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using StarterAssets;

public class GhostController : MonoBehaviour
{
    [Header("Phase Level")]
    public PhaseLevel phaseLevel;

    [Header("Player Collider")]
    public Collider playerCollider;
    
    [Header("FullScreenTest Controller")]
    [SerializeField] private float _phasingDisplayTime = 5.0f;
    [SerializeField] private float _phasingFadeOutTime = 0.5f;
    [SerializeField] private ScriptableRendererFeature _fullScreenPhasing;
    [SerializeField] private Material _material;
    [SerializeField] private float _voronoIntensityStat = 2.5f;
    [SerializeField] private float _vignetteIntensityStat = 1.25f;

    [Header("Audio")]
    public AudioClip enterAudioClip;
    public AudioClip duringAudioClip;
    public AudioClip exitAudioClip;
    public AudioSource audioSource;

    [Header("Post Processing")]
    public bool usePostProcessingFX;
    public float bloom;
    public float chromaticAberrations;
    public float vignette;

    private int _voronoIntensity;
    private int _vignetteIntensity;

    private CharacterMovement iaControls;

    //--Delegate events--
    public delegate void OnEnterPhase();
    public static event OnEnterPhase onEnterPhase;

    public delegate void OnExitPhase();
    public static event OnExitPhase onExitPhase;

    private bool phaseOn;
    private InputAction phase;
    private Vector3 startPosition;

    private void Awake()
    {
        phaseOn = false;
        iaControls = new CharacterMovement();
        phase = iaControls.CharacterControls.Phase;

        audioSource = GetComponent<AudioSource>();
        phaseLevel._ghostController = this;

        _voronoIntensity = Shader.PropertyToID("_VoronoIntensity");
        _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");
    }

    void Update()
    {
        //get input
        if (phase.triggered)
        {
            ToggleAbility();
        }
    }
    private void ToggleAbility()
    {
        if (phaseOn)
        {
            TurnPhaseOff();
        }
        else
        {
            if (phaseLevel.CanPhase())
                TurnPhaseOn();
        }

    }
    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
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

    public void ForceOutOfPhase()
    {
        TurnPhaseOff();
    }

    private void TurnPhaseOn()
    {
        phaseOn = true;
        phaseLevel.StartUsingPhase();

        //store start position
        startPosition = transform.position;

        //delegate events
        onEnterPhase?.Invoke();

        //post processing
        if (usePostProcessingFX)
        {
            PostProcess.Main._mBloomIntensity.Impulse(bloom);
            PostProcess.Main._mChromaticAberration.Impulse(chromaticAberrations);
            PostProcess.Main._mMVignette.Impulse(vignette);
        }

        //audio
        PlayAudio(enterAudioClip);

        //full screen fx
        _fullScreenPhasing.SetActive(true);
        _material.SetFloat(_voronoIntensity, _voronoIntensityStat);
        _material.SetFloat(_vignetteIntensity, _vignetteIntensityStat);
    }

    private void TurnPhaseOff()
    {
        phaseOn = false;
        phaseLevel.StopUsingPhase();

        //if inside something, teleport to start position
        if (InsideAnObject())
        {
            transform.position = startPosition;

            //a little freeze to prevent disorienting the player
            ThirdPersonController.Main.LockPlayerForDuration(0.2f);
        }

        //delegate events
        onExitPhase?.Invoke();

        //audio
        PlayAudio(exitAudioClip);

        //full screen fx
        _fullScreenPhasing.SetActive(false);
    }

    private bool InsideAnObject()
    {
        return true;
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
