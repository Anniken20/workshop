using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthBarImage; 
    public DeathScreenManager deathScreenManager;

    [Header("Time Stats")]
    [SerializeField] private float _damageDisplayTime = 5.0f;
    [SerializeField] private float _damageFadeOutTime = 0.5f;

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _fullScreenDamage;
    [SerializeField] private Material _material;

    [Header("Intensity Stats")]
    [SerializeField] private float _voronoIntensityStat = 2.5f;
    [SerializeField] private float _vignetteIntensityStat = 1.25f;

    private int _voronoIntensity = Shader.PropertyToID("_VoronoIntensity");
    private int _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        _fullScreenDamage.SetActive(false); // Set it inactive initially
    }
    public void UpdateHealthUI()
    {
    if (healthBarImage != null)
        {
            float fillAmount = (float)currentHealth / maxHealth; // Calculate fill amount based on health
            healthBarImage.fillAmount = fillAmount; // Update the fill amount of the image
        }
    }


    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;
        UpdateHealthUI();
        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Damage());
        }
    }

    private IEnumerator Damage()
    {
        _fullScreenDamage.SetActive(true);
        _material.SetFloat(_voronoIntensity, _voronoIntensityStat);
        _material.SetFloat(_vignetteIntensity, _vignetteIntensityStat);

        yield return new WaitForSeconds(_damageDisplayTime);

        float elapsedTime = 0f;
        while (elapsedTime < _damageFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedVoronoi = Mathf.Lerp(_voronoIntensityStat, 0f, (elapsedTime / _damageFadeOutTime));
            float lerpedVignette = Mathf.Lerp(_vignetteIntensityStat, 0f, (elapsedTime / _damageFadeOutTime));

            _material.SetFloat(_voronoIntensity, lerpedVoronoi);
            _material.SetFloat(_vignetteIntensity, lerpedVignette);

            yield return null;
        }

        _fullScreenDamage.SetActive(false);
    }

    public void Die()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        GetComponent<PlayerRespawn>().Die();
    }
}
