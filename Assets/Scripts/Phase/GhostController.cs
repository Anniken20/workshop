using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using StarterAssets;

public class GhostController : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("Inputting within this frame of another phase input will be ignored. Prevents spamming.")]
    public float minTimeBeforeSwitch;

    [Header("Phase Level")]
    public PhaseLevel phaseLevel;

    [Header("Gameplay Settings")]
    [Tooltip("The player will be locked from moving for this long" +
        " after being sent back to their original phase position" +
        " if phase ended inside an object. ")] public float lockedDuration = 0.2f;
    
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

    [Header("Default Ghost Shader")]
    public Shader theGhostShader;

    private CharacterMovement iaControls;

    //--Delegate events--
    public delegate void OnEnterPhase();
    public static event OnEnterPhase onEnterPhase;

    public delegate void OnExitPhase();
    public static event OnExitPhase onExitPhase;


    //more private fields
    private bool phaseOn;
    private InputAction phase;
    private Vector3 startPosition;

    private bool inAnObject = false;
    private bool canInput = true;


    //statics (fields that belong to the class rather than the object)
    public static Shader defaultGhostShader;
    public static Color defaultGhostColor = new Color(0, 1f, 0.949019f);

    private void Awake()
    {
        phaseOn = false;
        iaControls = new CharacterMovement();
        phase = iaControls.CharacterControls.Phase;

        audioSource = GetComponent<AudioSource>();
        phaseLevel._ghostController = this;

        _voronoIntensity = Shader.PropertyToID("_VoronoIntensity");
        _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

        defaultGhostShader = theGhostShader;
        //Shader.Find("Shader Graphs/GhostUnlitAttempt");
    }

    private void Update()
    {
        //get input
        if (!canInput) return;
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
            StartCoroutine(InputWaitRoutine());
        }
        else
        {
            if (phaseLevel.CanPhase())
            {
                TurnPhaseOn();
                StartCoroutine(InputWaitRoutine());
            }    
        }
    }

    private IEnumerator InputWaitRoutine()
    {
        canInput = false;
        yield return new WaitForSeconds(minTimeBeforeSwitch);
        canInput = true;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    /*
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
    */

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
            ThirdPersonController.Main.LockPlayerForDuration(lockedDuration);

            inAnObject = false;

            Camera.main.GetComponent<CameraController>().RecomposeCamera();
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
        return inAnObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!phaseOn) return;
        if (other.gameObject.GetComponent<PhaseThroughObject>() != null)
        {
            inAnObject = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!phaseOn) return;
        if (other.gameObject.GetComponent<PhaseThroughObject>() != null)
        {
            inAnObject = false;
        }
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
