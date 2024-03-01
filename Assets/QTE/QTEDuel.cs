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
    public TMP_Text PopupText;
    public float timeToShoot;
    [SerializeField] private Enemy enemyScript;
    public CinemachineVirtualCamera shoulderCamera;
    public Volume postProcessVolume;
    public DuelCameraController duelCameraController;

    [Header("Difficulty")]
    [Tooltip("The player will have at least this much time to fire. In seconds")]
    private float lowBoundTimeToShoot;
    [Tooltip("The player will have at most this much time to fire. In seconds")]
    private float highBoundTimeToShoot;
    [Tooltip("The player will wait at least this much time before being prompted to shoot. In seconds")]
    private float lowBoundTimeToPopup;
    [Tooltip("The player will wait at most this much time before being prompted to shoot. In seconds")]
    private float highBoundTimeToPopup;

    [Header("Aesthetics")]
    public float duelTimeScale = 0.5f;
    public float postTransitionDuration = 1f;
    public int lowPassCutoff = 800;


    //private variables -------------------------

    private int QTEGen;
    private bool inDuel;

    //input
    public CharacterMovement iaControls;
    private InputAction shootInput;
    private InputAction lassoInput;
    private InputAction phaseInput;

    private Tween timeTween;

    private void OnEnable()
    {
        if (enemyScript != null) {
            //subscribe to the enemy's death delegate so it only checks when the enemy takes damage.
            enemyScript.damageDelegate += CheckEnemyHealth;
        }

        iaControls = new CharacterMovement();
        shootInput = iaControls.CharacterControls.Shoot;
        lassoInput = iaControls.CharacterControls.Lasso;
        phaseInput = iaControls.CharacterControls.Phase;

        shootInput.Enable();
        lassoInput.Enable();
        phaseInput.Enable();
    }

    private void OnDisable()
    {
        if (enemyScript != null)
        {
            //unsubscribe to the enemy's death delegate
            enemyScript.damageDelegate -= CheckEnemyHealth;
        }

        shootInput.Disable();
        lassoInput.Disable();
        phaseInput.Disable();
    }

    public void CheckEnemyHealth()
    {
        //kick out if in a duel already. no need to check and enter more duels
        if (inDuel) return;

        float healthPercentage = enemyScript.currentHealth / enemyScript.maxHealth;

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
        enemyScript.Freeze();
        enemyScript.transform.LookAt(ThirdPersonController.Main.transform.position);
        enemyScript.GoToIdle();

        //generate one of the random 3 attacks
        QTEGen = Random.Range(0, 3);

        if (QTEGen == 0)
        {
            ThirdPersonController.Main.LockPlayerForDuration(timeToShoot);
            ShowPopupText("Left-Click!");
            StartCoroutine(InputRoutine(0));
        } else if(QTEGen == 1)
        {
            ThirdPersonController.Main.LockPlayerForDuration(timeToShoot);
            ShowPopupText("Right-Click!");
            StartCoroutine(InputRoutine(1));
        } else
        {
            ThirdPersonController.Main.LockPlayerForDuration(timeToShoot);
            ShowPopupText("T!");
            StartCoroutine(InputRoutine(2));
        }
    }
    private IEnumerator InputRoutine(int inputType)
    {
        float timeStep = 0.005f;
        float timeProgressed = 0f;

        while (timeProgressed < timeToShoot)
        {
            if (shootInput.triggered)
            {
                if (inputType == 0) StartCoroutine(Correct());
                else StartCoroutine(Failed());

                yield break;
            } else if (lassoInput.triggered)
            {
                if (inputType == 1) StartCoroutine(Correct());
                else StartCoroutine(Failed());

                yield break;
            } else if (phaseInput.triggered)
            {
                if (inputType == 2) StartCoroutine(Correct());
                else StartCoroutine(Failed());

                yield break;
            }
            // track how long we have been in the shooting window
            timeProgressed += timeStep;

            // wait a concrete time since timescale is changing
            //max 200fps
            yield return new WaitForSecondsRealtime(timeStep);
        }
        StartCoroutine(Failed());
    }

    void ShowPopupText(string message)
    {
        PopupText.text = message;
        StartCoroutine(HidePopupText());
    }

    IEnumerator HidePopupText()
    {
        yield return new WaitForSeconds(2f);
        PopupText.text = "";
    }

    IEnumerator Correct()
    {
        ShowPopupText("Correct!");

        yield return new WaitForSeconds(2f);
        EndDuel();
    }
    IEnumerator Failed()
    {
        ShowPopupText("Failed!");

        yield return new WaitForSeconds(2f);
        EndDuel();
    }

    private void EndDuel()
    {
        //turn on post processing
        DOTween.To(() => postProcessVolume.weight, x => postProcessVolume.weight = x, 0f, postTransitionDuration);

        //slow down time
        Time.timeScale = 1f;

        duelCameraController.Reset();

        PopupText.text = "";
        CameraController camController = Camera.main.GetComponent<CameraController>();
        enemyScript.Unfreeze();
        if (camController != null) camController.SwitchCameraView(true);
        inDuel = false;
        AudioManager.main.SetMusicLowPassFilter();
    }

    private void StartDuelAesthetics()
    {
        //turn on post processing
        DOTween.To(() => postProcessVolume.weight, x => postProcessVolume.weight = x, 1f, postTransitionDuration);

        //slow down time
        timeTween.SetUpdate(true);
        timeTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, duelTimeScale, postTransitionDuration);

        duelCameraController.StartDuel(highBoundTimeToPopup);
        AudioManager.main.SetMusicLowPassFilter(lowPassCutoff);

        //ThirdPersonController.Main.LockPlayerForDuration(2f);

        //Camera.main is a built in singleton for Unity. Singletons are cool look them up. 
        //you can access it from any script without needing to attach a reference.
        //attaching a reference works great too and is good practice. 
        //this is just a good strategy to avoid having to attach references for everything. 
        CameraController camController = Camera.main.GetComponent<CameraController>();

        //the parameter should be FALSE in order to switch to the shoulder cam
        if (camController != null) camController.SwitchCameraView(false);

        //then set the shoulder cam to focus on the enemy
        shoulderCamera.LookAt = enemyScript.transform;
    }
}
