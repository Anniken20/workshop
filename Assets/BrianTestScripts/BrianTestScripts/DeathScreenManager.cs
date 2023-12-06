using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject deathScreen;
    public static DeathScreenManager main;
    private PlayerRespawn respawner;
    public AudioClip drawTextSound;
    [SerializeField] GameObject startingButton;

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
        EventSystem.current.SetSelectedGameObject(startingButton);
        deathScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(ShowDeathScreenRoutine());

        //this method handles the global time scale and pause boolean
        //so that things like shooting arent allowed
        PauseMenu.main.PauseNoUI();
    }

    private IEnumerator ShowDeathScreenRoutine()
    {
        Color whiteCol = new(1, 1, 1, 1f);
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < texts.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            if (i == 1) yield return new WaitForSecondsRealtime(1f);
            texts[i].color = whiteCol;
            AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, drawTextSound);

            //couldn't get the color fade in to work
            //DOTween.To(()=> texts[i].color, x=> texts[i].color = x, new Color(1, 1, 1, 1), 1f);
        }
    }

    public void HideDeathScreen()
    {
        PauseMenu.main.UnPauseNoUI();
        deathScreen.SetActive(false);

        //this method handles the global time scale and pause boolean
        //so that things like shooting arent allowed
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
