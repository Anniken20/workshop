using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

public class QTETest : MonoBehaviour
{
    public TMP_Text PopupText; // Reference to the pop-up text element
    public float timeToShoot;

    [SerializeField]
    private Enemy enemyScript;

    private int currentEnemyIndex = 0;
    private int WaitingForKey;
    private int QTEGen;
    private bool hasProcessedInput = false;
    private bool inDuel;

    // Reference to the CameraController script
    public CameraController cameraController;

    //Reference to specifically the shoulder cam
    public CinemachineVirtualCamera shoulderCamera;

   // public CharacterMovement iaControls;
   // private InputAction shoot;

    void Start()
    {
        if (enemyScript != null) {

            //subscribe to the enemy's death delegate so it only checks when the enemy takes damage.
            enemyScript.damageDelegate += CheckEnemyHealth;
        }
        else
        {
            Debug.Log("No enemy scripts set!");
        }
    }


    public void CheckEnemyHealth()
    {
        //kick out if in a duel already. no need to check and enter more duels
        if (inDuel) return;

        float healthPercentage = 100 * enemyScript.currentHealth / enemyScript.maxHealth;

        if (healthPercentage <= 66f && healthPercentage > 33f)
        {
            StartQTE("[U]");
            Debug.Log("START QTE");
        }
        else if (healthPercentage <= 33f && healthPercentage > 0f)
        {
            StartQTE("[O]");
            Debug.Log("START QTE");
        }
        else if (healthPercentage <= 0f)
        {
            StartQTE("[L]");
            Debug.Log("START QTE");
        }
    }
    void StartQTE(string key)
    {
        inDuel = true;

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
        Debug.Log("QTEGen is " + QTEGen);


        if(QTEGen == 0)
        {
            ShowPopupText("PRESS U");
            StartCoroutine(InputRoutine(KeyCode.U));
        } else if(QTEGen == 1)
        {
            ShowPopupText("PRESS O");
            StartCoroutine(InputRoutine(KeyCode.O));
        } else
        {
            ShowPopupText("PRESS L");
            StartCoroutine(InputRoutine(KeyCode.L));
        }

        /*
        if (WaitingForKey == 0)
        {
            // Initial state: Nothing is activated
            QTEGen = Random.Range(1, 3);

            // Reset the input processing flag when QTE starts
            ResetInputProcessing();

            // Change camera view when QTE starts
            if (cameraController != null)
            {
                cameraController.SwitchCameraView(true);
            }

            ShowPopupText("Press " + key);
            WaitingForKey = 1; // Move to the QTE activation state
        }
        else if (WaitingForKey == 1)
        {
            // QTE activated, waiting for player input
            CheckInput();
        }
        else if (WaitingForKey == 2)
        {
            // Player succeeded in QTE
            // Switch back to main camera only if QTE state is still 2 (QTE is not overridden by another QTE)
            if (WaitingForKey == 2 && cameraController != null)
            {
                cameraController.SwitchCameraView(false); // Switch back to isometric view
            }

            // Reset QTE state and input processing flag
            WaitingForKey = 0;
            ResetInputProcessing();
        }
        else if (WaitingForKey == 3)
        {
            // Player failed in QTE and died
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        */
    }
    private IEnumerator InputRoutine(KeyCode shootKey)
    {
        float timeProgressed = 0f;

        // Pause the game
        Time.timeScale = 0;

        while (timeProgressed < timeToShoot)
        {
            // track how long we have been in the shooting window
            timeProgressed += Time.unscaledDeltaTime;

            // if a key is pressed
            if (Input.anyKeyDown)
            {
                // and the key down is the correct input
                if (Input.GetKeyDown(shootKey))
                {
                    StartCoroutine(Correct());

                    // leave this coroutine
                    yield break;
                }
                // is not the correct input
                else
                {
                    StartCoroutine(Failed());

                    // leave this coroutine
                    yield break;
                }
            }

            // wait a frame before resuming the while loop
            // nifty trick for coroutines
            yield return null;
        }

        Debug.Log("Took too long!");
        StartCoroutine(Failed());
    }

    void CheckInput()
    {
        if (WaitingForKey == 1)
        {
            if (Input.GetKeyDown(KeyCode.U) && QTEGen == 1)
            {
                StartCoroutine(Correct());
                Debug.Log("Correct");
            }
            else if (Input.GetKeyDown(KeyCode.O) && QTEGen == 2)
            {
                StartCoroutine(Correct());
                Debug.Log("Correct");
            }
            else if (Input.GetKeyDown(KeyCode.L) && QTEGen == 3)
            {
                StartCoroutine(Correct());
                Debug.Log("Correct");
            }
            else if (!hasProcessedInput)
            {
                hasProcessedInput = true;
                StartCoroutine(Failed());
                Debug.Log("Fail");
            }
        }
    }
    void ResetInputProcessing()
    {
        hasProcessedInput = false; // Reset the input processing flag
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

        // Resume the game
        Time.timeScale = 1;

        yield return new WaitForSeconds(2f);
        PopupText.text = "";
        CameraController camController = Camera.main.GetComponent<CameraController>();
        if (camController != null) camController.SwitchCameraView(true);
        inDuel = true;
    }
    IEnumerator Failed()
    {
        ShowPopupText("Failed!");

        // Resume the game
        Time.timeScale = 1;

        yield return new WaitForSeconds(2f);
        PopupText.text = "";
        CameraController camController = Camera.main.GetComponent<CameraController>();
        if(camController != null) camController.SwitchCameraView(true);
        inDuel = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //ResetInputProcessing(); // Reset input processing after coroutine completion
    }

    /*private void Awake()
    {
        iaControls = new CharacterMovement();
    }
    private void OnEnable()
    {
        shoot = iaControls.CharacterControls.Shoot;

        shoot.Enable();
    }
    private void OnDisable()
    {
        shoot.Disable();
    }*/
}
