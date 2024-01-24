using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;


public class FullScreenTestController : MonoBehaviour
{
    [Header("Time Stats")]
    [SerializeField] private float _phasingDisplayTime = 5.0f;
    [SerializeField] private float _phasingFadeOutTime = 0.5f;

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _fullScreenPhasing;
    [SerializeField] private Material _material;

    [Header("Intensity Stats")]
    [SerializeField] private float _voronoIntensityStat = 2.5f;
    [SerializeField] private float _vignetteIntensityStat = 1.25f;

    private int _voronoIntensity = Shader.PropertyToID("_VoronoIntensity");
    private int _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            StartCoroutine(Phasing());
        }
    }
    private void Start()
    {
        _fullScreenPhasing.SetActive(false);
    }

    private IEnumerator Phasing()
    {
        _fullScreenPhasing.SetActive(true);
        _material.SetFloat(_voronoIntensity, _voronoIntensityStat);
        _material.SetFloat(_vignetteIntensity, _vignetteIntensityStat);

        yield return new WaitForSeconds(_phasingDisplayTime);

        float elapsedTime = 0f;
        while(elapsedTime < _phasingFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedVoronoi = Mathf.Lerp(_voronoIntensityStat, 0f, (elapsedTime / _phasingFadeOutTime));
            float lerpedVignette = Mathf.Lerp(_vignetteIntensityStat, 0f, (elapsedTime / _phasingFadeOutTime));

            _material.SetFloat(_voronoIntensity, lerpedVoronoi);
            _material.SetFloat(_vignetteIntensity, lerpedVignette);

            yield return null;
        }
        
        _fullScreenPhasing.SetActive(false);
    }
    
}