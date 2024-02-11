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
    //private Color defaultColor;
    private new void Start()
    {
        base.Start();
        myRenderer = GetComponentInChildren<Renderer>();
        originalLayer = gameObject.layer;
        shaderOG = myRenderer.material.shader;
        /*if(myRenderer.material.color != null)
        {
            defaultColor = myRenderer.material.color;
        }*/
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
    //For example: What if the player phases and then walks away?
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
                if (ghostMode)
                {
                    mat.material.shader = GhostController.defaultGhostShader;
                    mat.material.color = GhostController.defaultGhostColor;
                } else
                {
                    mat.material.shader = shaderOG;
                    //mat.material.color = defaultColor;
                }
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
