using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject profilesMenu;
    public GameObject controlsMenu;

    public void PlayButton()
    {
        profilesMenu.SetActive(true);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        gameObject.SetActive(false);
        //SceneManager.LoadScene("VSLevel");
        //SceneManager.LoadScene("IntroCutscene");
    }

    public void SettingsButton()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        //SceneManager.LoadScene("VSLevel");
        SceneManager.LoadScene("IntroCutscene");
    }

    public void LoadHub()
    {
        SceneManager.LoadScene("HUB");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void QuitGame() 
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }

}
