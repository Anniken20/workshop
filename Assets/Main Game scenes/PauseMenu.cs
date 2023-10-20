using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PausePanel;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
    public void Restart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Debug.Log("Killed myself");
        Application.Quit();
    }
}
