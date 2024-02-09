using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* For default objects that you can pass through when phasing
 * 
 * Made by Caden and Amadin
 * Refactored 2/8/2024
 */

public class PhaseThroughObject : PhaseObject
{
    private int originalLayer;
    private Renderer myRenderer;
    private Shader shaderOG;
    private Shader shaderGhost;
    private new void Start()
    {
        base.Start();
        myRenderer = GetComponentInChildren<Renderer>();
        originalLayer = gameObject.layer;
        shaderGhost = Shader.Find("Shader Graphs/GhostUnlitAttempt");
        shaderOG = Shader.Find("Shader Graphs/LIT TOON");
    }
    protected override void OnEnter()
    {
        SwitchMaterial();
        ActivateCollider(false);
    }

    protected override void OnExit()
    {
        SwitchMaterial(false);
        ActivateCollider();
    }

    //I'm wary of using this function because there is potential to have this desync. 
    //For example: What if the player phases and then walks on-screen?
    private bool IsObjectVisible()
    {
        if (myRenderer == null)
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (!GeometryUtility.TestPlanesAABB(planes, myRenderer.bounds))
        {
            // Get in loser we are going phasing
            return false;
        }
        return true;
    }

    public void SwitchMaterial(bool ghostMode = true)
    {
        Renderer[] allMats = myRenderer.GetComponentsInChildren<Renderer>();

        foreach (Renderer mat in allMats)
        {
            if (mat.material.shader != null)
            {
                mat.material.shader = ghostMode ? shaderGhost : shaderOG;
            }
        }
    }

    public void ActivateCollider(bool yes = true)
    {
        Collider coll = GetComponent<Collider>();
        if (coll != null)
        {
            if (yes)
            {
                coll.isTrigger = false;
                gameObject.layer = originalLayer;
            } else
            {
                coll.isTrigger = true;
                // Enable the BulletPassThrough layer during the phase
                gameObject.layer = LayerMask.NameToLayer("BulletPassThrough");
            }
        }
    }
}
