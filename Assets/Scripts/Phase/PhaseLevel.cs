using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }
    public void ChangePhaseBy(float delta)
    {
        currLevel += delta;
        if (!AtFullMeter())
        {
            if(rechargeRoutine != null) StopCoroutine(rechargeRoutine);
            rechargeRoutine = StartCoroutine(RechargeRoutine());
        }
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
            Debug.Log("filling up meter");
            currLevel += rechargeRate * Time.deltaTime;
            UpdateMeter();

            //wait a frame before resuming loop
            yield return null;
        }
        canPhase = true;
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
        _ghostController.ForceOutOfPhase();
    }

    private void UpdateMeter()
    {
        abilityDurationBar.fillAmount = currLevel / maxLevel;
    }
}
