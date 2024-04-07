using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* A script to manage the phase level and meter.
 * Provides the public ChangePhaseBy(int amt) method for other classes to use. 
 * 
 * 
 * Based off the original script by Amadin
 * Refactored by Caden 2/10/2024
 */

public class PhaseLevel : MonoBehaviour
{
    [Header("Reference")]
    public Image abilityDurationBar;
    [HideInInspector] public GhostController _ghostController;

    [Header("Settings")]
    public float maxLevel;
    public float rechargeRate;
    public float spendRate;

    private float currLevel;
    private Coroutine rechargeRoutine;
    private Coroutine useRoutine;
    private Coroutine exhaustRoutine;
    private bool canPhase;

    private void Start()
    {
        if(maxLevel == 0)
        {
            maxLevel = 1f;
            rechargeRate = 0.2f;
            spendRate = 0.2f;
            
            Debug.LogWarning("GMHN Warning! Some PhaseLevel values were initialized to 0. Fixed.");
        }
        currLevel = maxLevel;
        canPhase = true;
        abilityDurationBar.fillAmount = currLevel / maxLevel;
    }
    public void ChangePhaseBy(float delta)
    {
        currLevel += delta;
        currLevel = Mathf.Clamp(currLevel, 0, maxLevel);
        if (!AtFullMeter())
        {
            if(rechargeRoutine != null) StopCoroutine(rechargeRoutine);
            rechargeRoutine = StartCoroutine(RechargeRoutine());
        }
        abilityDurationBar.fillAmount = currLevel / maxLevel;
    }

    public void StartUsingPhase()
    {
        if (useRoutine != null) StopCoroutine(useRoutine);
        if (rechargeRoutine != null) StopCoroutine(rechargeRoutine);
        useRoutine = StartCoroutine(UseRoutine());
    }

    public void StopUsingPhase()
    {
        if (rechargeRoutine != null) StopCoroutine(rechargeRoutine);
        if (useRoutine != null) StopCoroutine(useRoutine);
        rechargeRoutine = StartCoroutine(RechargeRoutine());
    }

    public bool CanPhase()
    {
        return canPhase;
    }

    public bool AtFullMeter()
    {
        return currLevel >= maxLevel;
    }

    public bool AtEmptyMeter()
    {
        return currLevel <= 0;
    }

    private IEnumerator RechargeRoutine()
    {
        while (!AtFullMeter())
        {
            currLevel += rechargeRate * Time.deltaTime;
            UpdateMeter();

            //wait a frame before resuming loop
            yield return null;
        }
        currLevel = Mathf.Clamp(currLevel, 0, maxLevel);
        if(!canPhase)
            _ghostController.ExhaustOver();
        canPhase = true;
        if (exhaustRoutine != null) StopCoroutine(exhaustRoutine);
    }

    private IEnumerator UseRoutine()
    {
        while (!AtEmptyMeter())
        {
            currLevel -= spendRate * Time.deltaTime;
            UpdateMeter();

            //wait a frame before resuming loop
            yield return null;
        }
        canPhase = false;
        currLevel = Mathf.Clamp(currLevel, 0, maxLevel);
        exhaustRoutine = StartCoroutine(ExhaustRoutine(false));
        _ghostController.ForceOutOfPhase();
    }

    private void UpdateMeter()
    {
        abilityDurationBar.fillAmount = currLevel / maxLevel;
    }

    private IEnumerator ExhaustRoutine(bool up)
    {
        if (up)
        {
            while (abilityDurationBar.color.a < 0.8f)
            {
                abilityDurationBar.color =
                    new Color(abilityDurationBar.color.r,
                    abilityDurationBar.color.g,
                    abilityDurationBar.color.b,
                    abilityDurationBar.color.a + (Time.deltaTime * 2f));

                yield return null;
            }
            abilityDurationBar.color =
                new Color(abilityDurationBar.color.r,
                abilityDurationBar.color.g,
                abilityDurationBar.color.b,
                0.8f);
        } else
        {
            while (abilityDurationBar.color.a > 0.2f)
            {
                abilityDurationBar.color =
                    new Color(abilityDurationBar.color.r,
                    abilityDurationBar.color.g,
                    abilityDurationBar.color.b,
                    abilityDurationBar.color.a - (Time.deltaTime * 2f));

                yield return null;
            }
            abilityDurationBar.color =
                new Color(abilityDurationBar.color.r,
                abilityDurationBar.color.g,
                abilityDurationBar.color.b,
                0.2f);
        }

        if (AtFullMeter())
        {
            abilityDurationBar.color =
                new Color(abilityDurationBar.color.r,
                abilityDurationBar.color.g,
                abilityDurationBar.color.b,
                1f);
        } else
        {
            exhaustRoutine = StartCoroutine(ExhaustRoutine(!up));
        }
    }
}
