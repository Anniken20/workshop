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
    public bool hasHealthBar = true;
    public Image fullHealthBar; // This is the static background health bar
    public Image currentHealthBar; // This is the shrinking/growing health bar
    public float healthBarFadeTime = 1f;

    [Header("Regen")]
    public bool regen;
    public int regenDMG;
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
        SetHealthBarVisible(false);
        if (debugText != null) debugText.text = currDmg + " / " + dmgTilBreak + " DMG";
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

    public void ApplyDamage(float dmg, Vector3 direction)
    {
        currDmg += dmg;

        if (!healthBarVisible)
        {
            SetHealthBarVisible(true);
            healthBarVisible = true;
        }

        // Update health bar
        if (currentHealthBar != null)
        {
            float healthPercentage = 1 - (currDmg / dmgTilBreak);
            currentHealthBar.fillAmount = Mathf.Clamp01(healthPercentage);
        }

        lastDamageTime = Time.time;
        isTakingDamage = true;

        if (currDmg >= dmgTilBreak)
        {
            Break();
            return; // No need to knockback
        }

        if (debugText != null) debugText.text = currDmg + " / " + dmgTilBreak + " DMG";
        Knockback(dmg, direction);

        if(regen && dmg < regenDMG){
            StartCoroutine(Regenerate(dmg));
        }
    }

    private void SetHealthBarVisible(bool isVisible)
    {
        if (fullHealthBar != null) fullHealthBar.enabled = isVisible;
        if (currentHealthBar != null) currentHealthBar.enabled = isVisible;
    }

    public void Knockback(float dmg, Vector3 direction)
    {
        Rigidbody rb;
        if (TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(direction * dmg * knockbackFactor, ForceMode.Impulse);
        }
    }

    public void Break()
    {
        if (breakController != null)
        {
            breakController.BreakIntoPieces();
        }
        else
        {
            Debug.LogError("BreakController component not found.");
            Destroy(gameObject);
        }
    }

    public IEnumerator Regenerate(float dmg)
    {
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