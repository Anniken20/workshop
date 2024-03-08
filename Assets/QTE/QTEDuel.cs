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
    [SerializeField] private Enemy duelEnemy;
    [SerializeField] private Enemy nextEnemy;
    public CinemachineVirtualCamera shoulderCamera;
    public Volume postProcessVolume;
    public DuelCameraController duelCameraController;
    public Image timeMeter;
    public Image timeMeterBG;
    public Image timeMeterFG;
    public CanvasGroup timeMeterCG;
    public ParticleSystem hitSparkSystem;
    public AudioSource gunShotAudio;
    public int enemyPhaseLevel;

    [Header("Difficulty")]
    [Tooltip("The player will have at least this much time to fire. In seconds")]
    public float lowBoundTimeToShoot;
    [Tooltip("The player will have at most this much time to fire. In seconds")]
    public float highBoundTimeToShoot;
    [Tooltip("The player will wait at least this much time before being prompted to shoot. In seconds")]
    public float lowBoundTimeToPopup;
    [Tooltip("The player will wait at most this much time before being prompted to shoot. In seconds")]
    public float highBoundTimeToPopup;
    [Tooltip("The amount of damage the enemy takes when player wins.")]
    public int enemyDamageOnLoss;
    [Tooltip("The amount of health the enemy restores when enemy wins.")]
    public int enemyRestoreOnWin;
    [Tooltip("The amount of damage the player takes when enemy wins.")]
    public int playerDamageOnLoss;
    [Tooltip("The amount of time before the player is released after a duel.")]
    public float releaseTime = 2f;

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

    private int phase = 1;

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

        if (phase == 1 && healthPercentage <= 0.66f && healthPercentage > 0.33f)
        {
            StartQTE();
        }
        else if (phase == 2 && healthPercentage <= 0.33f && healthPercentage > 0f)
        {
            StartQTE();
        }
        else if (phase == 3 && healthPercentage <= 0f)
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
        //yield return new WaitForSecondsRealtime(waitTime);

        //buffer in case player was spamming
        yield return new WaitForSecondsRealtime(2f);

        float endTime = generatedWaitTime + Time.realtimeSinceStartup;
        while (endTime > Time.realtimeSinceStartup)
        {
            //Debug.Log("endTime: " + endTime + ", currTime: " + Time.realtimeSinceStartup);
            if (shootInput.triggered)
            {
                ShowPopupText(tooEarlyString);
                StartCoroutine(Failed());
                yield break;
            }

            //timescale is changing
            yield return null;
        }
        ShowPopupText(fireString);
        generatedShootTime = Random.Range(lowBoundTimeToShoot, highBoundTimeToShoot);
        StartCoroutine(InputRoutine(generatedShootTime));
    }

    private IEnumerator InputRoutine(float shootTime)
    {
        float endTime = shootTime + Time.realtimeSinceStartup;

        timeMeterBG.gameObject.SetActive(true);
        timeMeterFG.gameObject.SetActive(true);
        PopupText.transform.DOShakePosition(0.3f, 15f).SetUpdate(true);
        timeMeter.DOFillAmount(1f, generatedShootTime - 0.1f).SetUpdate(true);

        while (endTime > Time.realtimeSinceStartup)
        {
            //Debug.Log("endTime: " + endTime + ", currTime: " + Time.realtimeSinceStartup);
            if (shootInput.triggered)
            {
                StartCoroutine(Correct());
                yield break;
            } 

            //timescale is changing
            yield return null;
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

        hitSparkSystem.gameObject.SetActive(true);
        hitSparkSystem.transform.position = new Vector3(0f, 1.5f) + 
            ThirdPersonController.Main.transform.position +
            ThirdPersonController.Main.transform.forward * 1.0f;
        hitSparkSystem.Play();
        gunShotAudio.Play();

        PlayerWonDuel();
        EndDuel();
        yield break;
    }
    private IEnumerator Failed()
    {
        gunShotAudio.Play();
        EnemyWonDuel();
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

        PopupText.transform.DOShakePosition(0.3f, 15f).SetUpdate(true);

        duelCameraController.Reset();

        CameraController camController = Camera.main.GetComponent<CameraController>();
        if (camController != null) camController.SwitchCameraView(true);
        inDuel = false;
        AudioManager.main.SetMusicLowPassFilter();

        DOTween.To(() => timeMeterCG.alpha, x => timeMeterCG.alpha = x, 0f, postTransitionDuration*2);

        //duel canvas
        Invoke(nameof(TurnoffDuelCanvas), postTransitionDuration);

        //free player
        Invoke(nameof(FreePlayer), releaseTime);
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

        topBarTween = cinematicBar_Top.transform.DOMove(
            new Vector3(cinematicBarToPos_Top.transform.position.x,
            cinematicBarToPos_Top.position.y), postTransitionDuration * 5f).SetEase(Ease.OutCubic);
        bottomBarTween = cinematicBar_Bottom.transform.DOMove(
            new Vector3(cinematicBarToPos_Bottom.transform.position.x,
            cinematicBarToPos_Bottom.position.y), postTransitionDuration*5f).SetEase(Ease.OutCubic);

        topBarTween.SetUpdate(true);
        bottomBarTween.SetUpdate(true);

        PopupText.transform.DOMoveX(PopupText.transform.position.x - 200f, highBoundTimeToPopup + 3f).SetUpdate(true);
        timeMeterCG.transform.DOMoveX(PopupText.transform.position.x - 200f, highBoundTimeToPopup + 3f).SetUpdate(true);
    }

    private void TurnoffDuelCanvas()
    {
        textCanvas.gameObject.SetActive(false);
    }

    private void PlayerWonDuel()
    {
        phase++;
        duelEnemy.TakeDamage(enemyDamageOnLoss);

        if(enemyPhaseLevel < 3 || nextEnemy == null)
        {
            nextEnemy.gameObject.SetActive(true);
            nextEnemy.transform.position = transform.position;
            nextEnemy.currentHealth = duelEnemy.currentHealth;

            gameObject.SetActive(false);
        } else
        {
            duelEnemy.Die();
        }
        //duelEnemy.PlayerWonDuel();
    }

    private void EnemyWonDuel()
    {
        ThirdPersonController.Main.gameObject.GetComponent<PlayerHealth>().TakeDamage(playerDamageOnLoss);
        duelEnemy.TakeDamage(-enemyRestoreOnWin);
        //duelEnemy.EnemyWonDuel();
    }

    private void FreePlayer()
    {
        ThirdPersonController.Main.ForceStopConversation();
    }
}
