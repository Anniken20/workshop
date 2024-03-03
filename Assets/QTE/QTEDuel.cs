using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
using StarterAssets;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class QTEDuel : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the pop-up text element")]
    public Canvas textCanvas;
    public TMP_Text PopupText;
    public GameObject cinematicBar_Top;
    public Transform cinematicBarToPos_Top;
    public GameObject cinematicBar_Bottom;
    public Transform cinematicBarToPos_Bottom;
    [SerializeField] private DuelEnemy duelEnemy;
    public CinemachineVirtualCamera shoulderCamera;
    public Volume postProcessVolume;
    public DuelCameraController duelCameraController;
    public Image timeMeter;
    public Image timeMeterBG;
    public Image timeMeterFG;
    public CanvasGroup timeMeterCG;

    [Header("Difficulty")]
    [Tooltip("The player will have at least this much time to fire. In seconds")]
    public float lowBoundTimeToShoot;
    [Tooltip("The player will have at most this much time to fire. In seconds")]
    public float highBoundTimeToShoot;
    [Tooltip("The player will wait at least this much time before being prompted to shoot. In seconds")]
    public float lowBoundTimeToPopup;
    [Tooltip("The player will wait at most this much time before being prompted to shoot. In seconds")]
    public float highBoundTimeToPopup;

    [Header("Aesthetics")]
    public float duelTimeScale = 0.5f;
    public float postTransitionDuration = 1f;
    public int lowPassCutoff = 800;

    [Header("Text Strings")]
    public string readyString;
    public string fireString;
    public string tooEarlyString;
    public string wonDuelString;
    public string tooLateString;

    //private variables -------------------------
    private bool inDuel;
    private float generatedWaitTime;
    private float generatedShootTime;

    //input
    public CharacterMovement iaControls;
    private InputAction shootInput;

    private Tween timeTween;
    private Tween topBarTween;
    private Tween bottomBarTween;

    private void OnEnable()
    {
        if (duelEnemy != null) {
            //subscribe to the enemy's death delegate so it only checks when the enemy takes damage.
            duelEnemy.damageDelegate += CheckEnemyHealth;
        }

        iaControls = new CharacterMovement();
        shootInput = iaControls.CharacterControls.Shoot;
        shootInput.Enable();
    }

    private void OnDisable()
    {
        if (duelEnemy != null)
        {
            //unsubscribe to the enemy's death delegate
            duelEnemy.damageDelegate -= CheckEnemyHealth;
        }

        shootInput.Disable();
    }

    public void CheckEnemyHealth()
    {
        //kick out if in a duel already. no need to check and enter more duels
        if (inDuel) return;

        float healthPercentage = duelEnemy.currentHealth / duelEnemy.maxHealth;

        if (healthPercentage <= 0.66f && healthPercentage > 0.33f)
        {
            StartQTE();
        }
        else if (healthPercentage <= 0.33f && healthPercentage > 0f)
        {
            StartQTE();
        }
        else if (healthPercentage <= 0f)
        {
            StartQTE();
        }
    }
    private void StartQTE()
    {
        StartDuelAesthetics();

        inDuel = true;
        duelEnemy.StartDuel();

        generatedWaitTime = Random.Range(lowBoundTimeToPopup, highBoundTimeToPopup);
        ThirdPersonController.Main.ForceStartConversation();
        ShowPopupText(readyString);
        StartCoroutine(WaitForPopupRoutine(generatedWaitTime));
    }

    private IEnumerator WaitForPopupRoutine(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        ShowPopupText(fireString);
        generatedShootTime = Random.Range(lowBoundTimeToShoot, highBoundTimeToShoot);
        StartCoroutine(InputRoutine(generatedShootTime));
    }

    private IEnumerator InputRoutine(float shootTime)
    {
        float timeStep = 0.005f;
        float timeProgressed = 0f;

        timeMeterCG.alpha = 1f;
        timeMeter.DOFillAmount(1f, generatedShootTime - 0.1f).SetUpdate(true);

        while (timeProgressed < shootTime)
        {
            Debug.Log("time progressed: " + timeProgressed);
            if (shootInput.triggered)
            {
                StartCoroutine(Correct());
                yield break;
            } 
            // track how long we have been in the shooting window
            timeProgressed += timeStep;

            // wait a concrete time since timescale is changing
            //max 200fps
            yield return new WaitForSecondsRealtime(timeStep);
        }
        ShowPopupText(tooLateString);
        StartCoroutine(Failed());
    }

    private void ShowPopupText(string message)
    {
        textCanvas.gameObject.SetActive(true);
        PopupText.text = message;
        StartCoroutine(HidePopupText());
    }

    private IEnumerator HidePopupText()
    {
        yield return new WaitForSeconds(2f);
        PopupText.text = "";
        textCanvas.gameObject.SetActive(false);
    }

    private IEnumerator Correct()
    {
        ShowPopupText(wonDuelString);
        EndDuel();
        yield break;
    }
    private IEnumerator Failed()
    {
        EndDuel();
        yield break;
    }

    private void EndDuel()
    {
        //turn on post processing
        DOTween.To(() => postProcessVolume.weight, x => postProcessVolume.weight = x, 0f, postTransitionDuration);

        //fix time
        timeTween.Kill();
        Time.timeScale = 1f;

        duelCameraController.Reset();

        PopupText.text = "";
        CameraController camController = Camera.main.GetComponent<CameraController>();
        if (camController != null) camController.SwitchCameraView(true);
        inDuel = false;
        AudioManager.main.SetMusicLowPassFilter();
        ThirdPersonController.Main.ForceStopConversation();

        DOTween.To(() => timeMeterCG.alpha, x => timeMeterCG.alpha = x, 0f, postTransitionDuration);

        //duel canvas
        Invoke(nameof(TurnoffDuelCanvas), 10f);
    }

    private void StartDuelAesthetics()
    {
        //turn on duel canvas
        textCanvas.gameObject.SetActive(true);

        //turn on post processing
        DOTween.To(() => postProcessVolume.weight, x => postProcessVolume.weight = x, 1f, postTransitionDuration);

        //slow down time
        timeTween.SetUpdate(true);
        timeTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, duelTimeScale, postTransitionDuration);

        duelCameraController.StartDuel(highBoundTimeToPopup);
        AudioManager.main.SetMusicLowPassFilter(lowPassCutoff);

        //Camera.main is a built in singleton for Unity. Singletons are cool look them up. 
        //you can access it from any script without needing to attach a reference.
        //attaching a reference works great too and is good practice. 
        //this is just a good strategy to avoid having to attach references for everything. 
        CameraController camController = Camera.main.GetComponent<CameraController>();

        //the parameter should be FALSE in order to switch to the shoulder cam
        if (camController != null) camController.SwitchCameraView(false);

        //then set the shoulder cam to focus on the enemy
        shoulderCamera.LookAt = duelEnemy.transform;

        //duel canvas
        textCanvas.gameObject.SetActive(true);
        topBarTween = cinematicBar_Top.transform.DOMove(
            new Vector3(cinematicBarToPos_Top.transform.position.x,
            cinematicBarToPos_Top.position.y), postTransitionDuration * 5f).SetEase(Ease.OutCubic);
        bottomBarTween = cinematicBar_Bottom.transform.DOMove(
            new Vector3(cinematicBarToPos_Bottom.transform.position.x,
            cinematicBarToPos_Bottom.position.y), postTransitionDuration*5f).SetEase(Ease.OutCubic);

        topBarTween.SetUpdate(true);
        bottomBarTween.SetUpdate(true);
    }

    private void TurnoffDuelCanvas()
    {
        textCanvas.gameObject.SetActive(false);
    }
}
