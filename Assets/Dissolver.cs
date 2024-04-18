using System.Collections;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    private Renderer[] _childRenderers;
    private Material[] _childMaterials;
    public float duration = 3.0f;
    private static readonly int Dissolve = Shader.PropertyToID("_Dissolve"); // Use the correct dissolve property name

    public void InitAndDissolve()
    {
        InitializeMaterials();
        StartCoroutine(DissolveMaterialsRoutine());
    }

    private void InitializeMaterials()
    { 
        //Debug.Log("initmats called");
        _childRenderers = GetComponentsInChildren<Renderer>(true);
        _childMaterials = new Material[_childRenderers.Length];

        for (int i = 0; i < _childRenderers.Length; i++)
        { 
            //Debug.Log("im in loop assigning child " + i);
            Material mat = _childRenderers[i].material;
            
            // check if the material has the desired shader
            if (mat.shader.name == "Shader Graphs/GhostUnlitURPShader") 
            {
                _childMaterials[i] = mat;
                //Debug.Log("children assigned");
            }
        }
    }

    private IEnumerator DissolveMaterialsRoutine()
    { 
        //Debug.Log("dissolving");
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float dissolveValue = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            foreach (Material material in _childMaterials)
            {
                if (material != null)
                {
                    //Debug.Log("material " + material.shader.name + "set dissolve value to " + dissolveValue);
                    material.SetFloat(Dissolve, dissolveValue);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that all materials are fully dissolved at the end
        foreach (Material material in _childMaterials)
        {
            if (material != null)
            {
                material.SetFloat(Dissolve, 0f);
            }
        }
    }
}