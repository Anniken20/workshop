using UnityEngine;

/* For default objects that you can pass through when phasing
 * 
 * Made by Caden and Amadin
 * Refactored 2/8/2024
 */

public class AntiPhaseThroughObject : PhaseThroughObject
{
    private int originalLayerQ;
    private Renderer myRendererQ;
    private Shader shaderOGQ;
    //private Color defaultColor;
    private void Start()
    {
        myRendererQ = GetComponentInChildren<Renderer>();
        originalLayerQ = gameObject.layer;
        shaderOGQ = myRendererQ.material.shader;
        /*if(myRenderer.material.color != null)
        {
            defaultColor = myRenderer.material.color;
        }*/
    }
    protected override void OnEnter()
    {
        SwitchMaterial();
        ActivateCollider();
    }

    protected override void OnExit()
    {
        SwitchMaterial(false);
        ActivateCollider(false);
    }

    public new void SwitchMaterial(bool ghostMode = true)
    {
        Renderer[] allMats = myRendererQ.GetComponentsInChildren<Renderer>();

        foreach (Renderer mat in allMats)
        {
            if (mat.material.shader != null)
            {
                if (ghostMode)
                {
                    mat.material.shader = shaderOGQ;
                    //mat.material.color = defaultColor;
                }
                else
                {

                    mat.material.shader = GhostController.defaultGhostShader;
                    mat.material.color = GhostController.defaultGhostColor;
                }
            }
        }
    }

    public new void ActivateCollider(bool yes = true)
    {
        Collider coll = GetComponent<Collider>();
        if (coll != null)
        {
            if (yes)
            {
                coll.isTrigger = false;
                gameObject.layer = originalLayerQ;
            }
            else
            {
                coll.isTrigger = true;
                // Enable the BulletPassThrough layer during the phase
                gameObject.layer = LayerMask.NameToLayer("BulletPassThrough");
                //idk????
            }
        }
    }
}
