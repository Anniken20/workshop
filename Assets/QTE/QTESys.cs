using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using Random = UnityEngine.Random;

public class QTESys : MonoBehaviour
{
    public Text PopupText; // Reference to the pop-up text element

    [SerializeField]
    private MonoBehaviour[] enemyScripts;

    private MethodInfo getCurrentHealthPercentageMethod;
    private int currentEnemyIndex = 0;
    private int WaitingForKey;
    private int QTEGen;

    // Reference to the CameraController script
    public CameraController cameraController;

    void Start()
    {
        if (enemyScripts != null && enemyScripts.Length > 0)
        {
            SetCurrentEnemyScript();
        }
        else
        {
            Debug.LogError("No enemy scripts set!");
        }
    }

    void Update()
    {
        CheckEnemyHealth();
    }

    void CheckEnemyHealth()
    {
        if (getCurrentHealthPercentageMethod != null)
        {
            float healthPercentage = (float)getCurrentHealthPercentageMethod.Invoke(enemyScripts[currentEnemyIndex], null);

            if (healthPercentage <= 66f && healthPercentage > 33f)
            {
                StartQTE("[U]");
            }
            else if (healthPercentage <= 33f && healthPercentage > 0f)
            {
                StartQTE("[P]");
            }
            else if (healthPercentage <= 0f)
            {
                StartQTE("[L]");
            }
        }
        else
        {
            Debug.LogError("Enemy health component or method not set!");
        }
    }

    void SetCurrentEnemyScript()
    {
        if (currentEnemyIndex < enemyScripts.Length)
        {
            getCurrentHealthPercentageMethod = enemyScripts[currentEnemyIndex].GetType().GetMethod("GetCurrentHealthPercentage");
        }
        else
        {
            Debug.LogError("No more enemy scripts available!");
        }
    }

    void SwitchToNextEnemy()
    {
        currentEnemyIndex++;
        if (currentEnemyIndex < enemyScripts.Length)
        {
            SetCurrentEnemyScript();
        }
        else
        {
            Debug.LogError("No more enemy scripts available!");
        }
    }

    void SwitchEnemyButtonPressed()
    {
        SwitchToNextEnemy();
    }

    void StartQTE(string key)
    {
        if (WaitingForKey == 0)
        {
            QTEGen = Random.Range(1, 4);

            // Change camera view when QTE starts
            if (cameraController != null)
            {
                cameraController.SwitchCameraView();
            }

            ShowPopupText("Press " + key);
        }

        if (WaitingForKey == 1)
        {
            CheckInput();
        }
    }

    void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            if ((QTEGen == 1 && Input.GetButtonDown("U")) ||
                (QTEGen == 2 && Input.GetButtonDown("P")) ||
                (QTEGen == 3 && Input.GetButtonDown("L")))
            {
                StartCoroutine(Correct());
            }
            else
            {
                StartCoroutine(Failed());
            }
        }
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
    }

    IEnumerator Failed()
    {
        ShowPopupText("Failed!");
        yield return new WaitForSeconds(2f);
        PopupText.text = "";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}