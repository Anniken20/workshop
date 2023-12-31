using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class MoonPhaseSystem : MonoBehaviour
{
    public delegate void FullMoonEvent();
    public static event FullMoonEvent OnFullMoon;

    public Image backgroundImage;
    public Image moonImage; 
    public TMP_Text phaseText;
    public TMP_Text enemyText;
    public ThirdPersonController player;
    public Enemy enemy;
    public GameObject enemyCam;
    public Scale HUD;

    public void PhaseAnimation()
    {
        backgroundImage.color = Color.clear;
        moonImage.color = Color.clear;
        phaseText.color = Color.clear;
        enemyText.color = Color.clear;
        StartCoroutine(PhaseAnimationRoutine());
    }

    private IEnumerator PhaseAnimationRoutine()
    {
        backgroundImage.DOColor(Color.black, 1f);
        yield return new WaitForSeconds(2f);
        moonImage.DOColor(Color.white, 1f);
        yield return new WaitForSeconds(2f);
        phaseText.DOColor(Color.white, 1f);
        yield return new WaitForSeconds(2f);
        enemyText.DOColor(Color.white, 1f);
        yield return new WaitForSeconds(2f);
        moonImage.DOColor(Color.clear, 1f);
        phaseText.DOColor(Color.clear, 1f);
        enemyText.DOColor(Color.clear, 1f);
        yield return new WaitForSeconds(1f);
        backgroundImage.DOColor(Color.clear, 1f);
        yield return new WaitForSeconds(1f);
        StartShowdown();
        HUD.ScaleTo(1f);
    }

    public void StartShowdown()
    {
        //may eventually cause enemy to transition to their running state or something
        player._inDialogue = false;
        enemyCam.GetComponent<CameraBlender>().DeactivateCamera();
    }

    /*
    private void Update()
    {
        // When it's a full moon, trigger the boss event?
        if (IsFullMoon())
        {
            OnFullMoon?.Invoke();
        }
    }

    private bool IsFullMoon()
    {
        // when is it full moon???
        // You might need data for moon phases or use a time-based approach we can do seconds or minute based?
        // Return true if it's a full moon, otherwise return false
        return false; 
    }
    */
}
