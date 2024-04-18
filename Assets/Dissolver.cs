using System.Collections;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    private Renderer[] _childRenderers;
    private Material[] _childMaterials;
    public float duration = 3.0f;
    private static readonly int Dissolve = Shader.PropertyToID("Dissolve");

    private void Start()
    {
        InitializeMaterials();
    }

    public void InitAndDissolve()
    {
        InitializeMaterials();
        StartCoroutine(DissolveMaterials());
    }

    private void InitializeMaterials()
    {
    _childRenderers = GetComponentsInChildren<Renderer>();
        _childMaterials = new Material[_childRenderers.Length];

        for (int i = 0; i < _childRenderers.Length; i++)
        {
            Material mat = _childRenderers[i].material;

            if (mat.shader.name == "ShaderGraph/GhostUnlitURPShader") 
            {
                _childMaterials[i] = mat;
            }
        }
        StartCoroutine(DissolveMaterials());
    }

     IEnumerator DissolveMaterials()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float dissolveValue = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            for (int i = 0; i < _childMaterials.Length; i++)
            {
                if (_childMaterials[i] != null)
                {
                    _childMaterials[i].SetFloat(Dissolve, dissolveValue); 
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < _childMaterials.Length; i++)
        {
            if (_childMaterials[i] != null)
            {
                _childMaterials[i].SetFloat(Dissolve, 0f); 
            }
        }
    }
}