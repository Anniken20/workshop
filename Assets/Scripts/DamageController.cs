using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/* Script for handling damage from bullets
 * 
 * 
 * 
 * Caden Henderson
 * 10/7/23
 */

[RequireComponent(typeof(Rigidbody))]
public class DamageController : MonoBehaviour, IShootable
{
    [Tooltip("Once the object reaches this level of damage, it is destroyed.")]
    public float dmgTilBreak;
    [Tooltip("Multiplies by the direction to increase knockback when hit by a bullet.")]
    public float knockbackFactor = 5f;
    public float startingDamage;

    [Header("Debug")]
    public TMP_Text debugText;

    [Header("Health Bar")]
    public bool hasHealthBar = true; // Option to enable or disable the health bar
    public Image fullHealthBar;
    public Image healthBarMask;
    public float healthBarFadeTime = 1f; // Time in seconds for health bar to fade
    [Header("Regen")]
    public bool regen;
    [Tooltip("Amount of damage needed to not regen")]
    public int regenDMG;
    [Tooltip("Time to wait before regen")]
    public float regenTimer;

    private float currDmg;
    public BreakController breakController;
    private bool isTakingDamage;
    private float lastDamageTime;
    private bool healthBarVisible;

    private void Start()
    {
        currDmg = startingDamage;
        breakController = GetComponent<BreakController>();
        lastDamageTime = Time.time;
        healthBarVisible = false; 

        if (debugText != null) debugText.text = currDmg + " / " + dmgTilBreak + " DMG";

        SetHealthBarVisible(false);
    }

    private void Update()
    {
        if (Time.time - lastDamageTime >= healthBarFadeTime && isTakingDamage)
        {
            isTakingDamage = false;
            SetHealthBarVisible(false);
            healthBarVisible = false; 
        }
    }

    // Apply damage, apply knockback, destroy if broken
    public void ApplyDamage(float dmg, Vector3 direction)
    {
        currDmg += dmg;

        if (!healthBarVisible && (fullHealthBar != null && healthBarMask != null))
        {
            SetHealthBarVisible(true);
            healthBarVisible = true; // Update visibility flag
        }

        if (fullHealthBar != null && healthBarMask != null)
        {
            // Update health bar
            float healthPercentage = 1 - (currDmg / dmgTilBreak);
            healthBarMask.fillAmount = Mathf.Clamp01(healthPercentage);

            lastDamageTime = Time.time;
            isTakingDamage = true;
        }
        if(regen && dmg < regenDMG){
            StartCoroutine(Regenerate(dmg));
        }

        if (currDmg >= dmgTilBreak)
        {
            Break();
            return; // No need to knockback
        }

        if (debugText != null) debugText.text = currDmg + " / " + dmgTilBreak + " DMG";

        Knockback(dmg, direction);
    }

    private void SetHealthBarVisible(bool isVisible)
    {
        if (fullHealthBar != null) fullHealthBar.enabled = isVisible;
        if (healthBarMask != null) healthBarMask.enabled = isVisible;
    }

    // Apply force to rigidbody to knockback
    public void Knockback(float dmg, Vector3 direction)
    {
        Rigidbody rb;
        if (TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(direction * dmg * knockbackFactor);
        }
    }

    // For now, just destroy. Could be something more complicated later
    private void Break()
    {
        if (breakController != null)
        {
            breakController.BreakIntoPieces();
        }
        else
        {
            Debug.LogError("BreakController component not found.");
            // If BreakController is not found, destroy the object
            Destroy(gameObject);
        }
    }
         public IEnumerator Regenerate(float dmg){
        yield return new WaitForSeconds(regenTimer);
        currDmg -= dmg;
    }

    public void OnShot(BulletController bullet)
    {
        //added this interface just so that the line renderer turns red.
        //dont feel like refactoring it so it actually uses OnShot
        //because then the bullets might try to apply both the shootable and DamageController
        //interactions.

        //teehee! - Caden
    }
}