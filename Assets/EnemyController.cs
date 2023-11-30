using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Camera mainCamera;
    public Camera deathCamera;
    public GameObject qteCanvas;
    public float qteDuration = 3f;
    public QTEScript qteScript; // Reference to the QTE script

    private enum EnemyState
    {
        Alive,
        Dead,
        QTE
    }

    private EnemyState currentState = EnemyState.Alive;

    private void Start()
    {
        // Disable death camera and QTE canvas initially
        deathCamera.gameObject.SetActive(false);
        qteCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentState == EnemyState.Alive)
        {
            // Enemy is killed
            Die();
        }
    }

    private void Die()
    {
        // Set the enemy state to QTE
        currentState = EnemyState.QTE;

        // Disable the main camera and enable the death camera
        mainCamera.gameObject.SetActive(false);
        deathCamera.gameObject.SetActive(true);

        // Show QTE canvas
        qteCanvas.SetActive(true);

        // Start the QTE in the QTE script
        qteScript.StartQTE();
    }

    // Called from QTEScript when QTE is successful
    public void FinishOffEnemy()
    {
        // Set the enemy state to Dead
        currentState = EnemyState.Dead;

        // Perform actions to finish off the enemy
        Debug.Log("Enemy finished off!");
        Destroy(gameObject);
    }
}

