using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfilesMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject profilesMenu;
    public GameObject controlsMenu;

    public void StartGameWithProfile(int profileIndex)
    {
        SceneManager.LoadScene("IntroCutscene_MP4");
        PlayerPrefs.SetInt("SelectedProfileIndex", profileIndex);
    }

}
