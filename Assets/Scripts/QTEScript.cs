using UnityEngine;
using System.Collections;

public class QTEScript : MonoBehaviour
{
    public float qteDuration = 5f; // Duration of the QTE in seconds
    public string qteButton = "Fire1"; // Button to press for the QTE

    private bool qteActive = false;

    void Update()
    {
        if (qteActive)
        {
            // Check for QTE input
            if (Input.GetButtonDown(qteButton))
            {
                QTESuccess();
            }
        }
    }

    public void StartQTE()
    {
        StartCoroutine(QTESequence());
    }

    private IEnumerator QTESequence()
    {
        qteActive = true;

        float timer = 0f;

        while (timer < qteDuration)
        {
            // Increment timer
            timer += Time.deltaTime;

            yield return null;
        }

        // QTE time ran out, call QTE failure
        QTEFailure();
    }

    private void QTESuccess()
    {
        qteActive = false;

        // QTE was successful, perform actions here
        Debug.Log("QTE Successful!");
    }

    private void QTEFailure()
    {
        qteActive = false;

        // QTE failed, perform actions here
        Debug.Log("QTE Failed!");
    }

    private void ResetScene()
    {
        Invoke("RestartScene", 2f);
    }

    private void RestartScene()
    {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
