using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
using StarterAssets;

public class QTETest : MonoBehaviour
{
    public TMP_Text PopupText; // Reference to the pop-up text element
    public float timeToShoot;

    [SerializeField]
    private Enemy enemyScript;

    private int WaitingForKey;
    private int QTEGen;
    private bool hasProcessedInput = false;
    private bool inDuel;

    // Reference to the CameraController script
    public CameraController cameraController;

    //Reference to specifically the shoulder cam
    public CinemachineVirtualCamera shoulderCamera;

    public CharacterMovement iaControls;
    private InputAction shootInput;
    private InputAction lassoInput;
    private InputAction phaseInput;

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
        inDuel = true;

        enemyScript.Freeze();
        enemyScript.transform.LookAt(ThirdPersonController.Main.transform.position);

        //ThirdPersonController.Main.LockPlayerForDuration(2f);

        //Camera.main is a built in singleton for Unity. Singletons are cool look them up. 
        //you can access it from any script without needing to attach a reference.
        //attaching a reference works great too and is good practice. 
        //this is just a good strategy to avoid having to attach references for everything. 
        CameraController camController = Camera.main.GetComponent<CameraController>();

        //the parameter should be FALSE in order to switch to the shoulder cam
        if (camController != null) cameraController.SwitchCameraView(false);

        //then set the shoulder cam to focus on the enemy
        shoulderCamera.LookAt = enemyScript.transform;

        //generate one of the random 3 attacks
        QTEGen = Random.Range(0, 3);
        
        enemyScript.GoToIdle();

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
            timeProgressed += Time.unscaledDeltaTime;

            // wait a frame before resuming the while loop
            // nifty trick for coroutines
            yield return null;
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
        WaitingForKey = 0;
    }

    IEnumerator Correct()
    {
        ShowPopupText("Correct!");


        yield return new WaitForSeconds(2f);
        PopupText.text = "";
        CameraController camController = Camera.main.GetComponent<CameraController>();
        enemyScript.Unfreeze();
        if (camController != null) camController.SwitchCameraView(true);
        inDuel = true;
    }
    IEnumerator Failed()
    {
        ShowPopupText("Failed!");

        yield return new WaitForSeconds(2f);
        PopupText.text = "";
        CameraController camController = Camera.main.GetComponent<CameraController>();
        enemyScript.Unfreeze();
        if (camController != null) camController.SwitchCameraView(true);
        inDuel = false;
    }
}
