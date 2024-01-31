using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

public class QTETest : MonoBehaviour
{
    public Text PopupText; // Reference to the pop-up text element

    [SerializeField]
    private Enemy enemyScript;

    private int currentEnemyIndex = 0;
    private int WaitingForKey;
    private int QTEGen;
    private bool hasProcessedInput = false;

    // Reference to the CameraController script
    public CameraController cameraController;

   // public CharacterMovement iaControls;
   // private InputAction shoot;

    void Start()
    {
        if (enemyScript != null) { 
        }
        else
        {
            Debug.Log("No enemy scripts set!");
        }
    }

    void Update()
    {
        CheckEnemyHealth();
        Debug.Log("CHECK ENEMY HEALTH");
    }

    void CheckEnemyHealth()
    {
        float healthPercentage = 100 * enemyScript.currentHealth / enemyScript.maxHealth;

        if (healthPercentage <= 64f && healthPercentage > 33f)
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
        StartCoroutine(HidePopupText());
        PopupText.text = message;
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
        WaitingForKey = 2; // QTE is complete
        yield return new WaitForSeconds(2f);
        PopupText.text = "";
        ResetInputProcessing(); // Reset input processing after coroutine completion
    }

    IEnumerator Failed()
    {
        ShowPopupText("Failed!");
        yield return new WaitForSeconds(2f);
        PopupText.text = "";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResetInputProcessing(); // Reset input processing after coroutine completion
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
