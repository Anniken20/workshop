using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathScreenManager : MonoBehaviour
{
    public GameObject deathScreen;
    public static DeathScreenManager main;
    private PlayerRespawn respawner;

    private void Awake()
    {
        //Singleton behavior
        if (main != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            main = this;
        }
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);

        //this method handles the global time scale and pause boolean
        //so that things like shooting arent allowed
        PauseMenu.main.PauseNoUI();
    }

    public void HideDeathScreen()
    {
        deathScreen.SetActive(false);

        //this method handles the global time scale and pause boolean
        //so that things like shooting arent allowed
        PauseMenu.main.UnPauseNoUI();
    }

    public void RestartFromCheckpoint()
    {
        respawner = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRespawn>();
        respawner.RestartFromCheckpoint();
        HideDeathScreen();
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        HideDeathScreen();
        SceneManager.LoadScene("MainMenu");
    }
}
