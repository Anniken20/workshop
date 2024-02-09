using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Controller")]

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
    public float barRegenerationRate = 0.2f; // Rate at which the ability bar regenerates per second

    private CharacterMovement iaControls;

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

    //--Delegate events--
    public delegate void OnEnterPhase();
    public static event OnEnterPhase onEnterPhase;

    public delegate void OnExitPhase();
    public static event OnExitPhase onExitPhase;

    //if (abilityDurationBar != null) abilityDurationBar.fillAmount = countdownTimer / abilityDuration;

    private bool phaseOn;
    private InputAction phase;

    private void Awake()
    {
        phaseOn = false;
        iaControls = new CharacterMovement();
        phase = iaControls.CharacterControls.Phase;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Regenerate ability bar over time
        /*
        if(abilityDurationBar != null) {
            if (!abilityEnabled && !isCooldownActive && abilityDurationBar.fillAmount < maxBarValue)
            {
                abilityDurationBar.fillAmount += barRegenerationRate * Time.deltaTime;
            }
        }
        */

        //get input
        if (phase.triggered)
        {
            ToggleAbility();
        }
    }
    void ToggleAbility()
    {
        phaseOn = !phaseOn;

        if (phaseOn)
        {
            TurnPhaseOn();         
        }
        else
        {
            TurnPhaseOff();
        }

    }
    void PlayAudio(AudioClip clip)
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

    private void TurnPhaseOn()
    {
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
        //delegate events
        onExitPhase?.Invoke();

        //audio
        PlayAudio(exitAudioClip);

        //full screen fx
        _fullScreenPhasing.SetActive(false);
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
