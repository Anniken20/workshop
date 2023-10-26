using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* Script for handling damage from bullets
 * 
 * 
 * 
 * Caden Henderson
 * 10/7/23
 */

[RequireComponent(typeof(Rigidbody))]
public class DamageController : MonoBehaviour
{
    [Tooltip("Once the object reaches this level of damage it is destroyed.")]
    public float dmgTilBreak;
    [Tooltip("Multiplies by the direction to increase knockback when hit by bullet.")]
    public float knockbackFactor = 5f;
    public float startingDamage;

    [Header("Debug")]
    public TMP_Text debugText;

    private float currDmg;

    public BreakController breakObject;


    private void Start()
    {
        currDmg = startingDamage;

        if (debugText != null) debugText.text = currDmg + " / " + dmgTilBreak + " DMG";
    }

    //apply damage, apply knockback, destroy if broken
    public void ApplyDamage(float dmg, Vector3 direction)
    {
        currDmg += dmg;
        //Debug.Log("currDmg: " + currDmg);
        if (currDmg >= dmgTilBreak)
        {
            Break();

            //return since no need to knockback
            return;
        }
        if (debugText != null) debugText.text = currDmg + " / " + dmgTilBreak + " DMG";
        Knockback(dmg, direction);
    }

    //apply force to rigidbody to knockback
    public void Knockback(float dmg, Vector3 direction)
    {
        Rigidbody rb;
        if(TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(direction * dmg * knockbackFactor);
        }
    }

    //for now just destroy. could be something more complicated later
    private void Break()
    {
        if (breakObject != null)
        {
            breakObject.BreakIntoPieces();
            //Destroy(gameObject);
        }
    }
}
