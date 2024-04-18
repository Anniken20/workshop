using System.Collections;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    private Renderer[] childRenderers;
    private Material[] childMaterials;
    public float duration = 3.0f; 

    private void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        childMaterials = new Material[childRenderers.Length];

        for (int i = 0; i < childRenderers.Length; i++)
        {
            Material mat = childRenderers[i].material;

            if (mat.shader.name == "ShaderGraph/GhostUnlitURPShader") 
            {
                childMaterials[i] = mat;
            }
        }
        StartCoroutine(DissolveMaterials());
    }

    private IEnumerator DissolveMaterials()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float dissolveValue = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            for (int i = 0; i < childMaterials.Length; i++)
            {
                if (childMaterials[i] != null)
                {
                    childMaterials[i].SetFloat("Dissolve", dissolveValue); 
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < childMaterials.Length; i++)
        {
            if (childMaterials[i] != null)
            {
                childMaterials[i].SetFloat("Dissolve", 0f); 
            }
        }
    }
}