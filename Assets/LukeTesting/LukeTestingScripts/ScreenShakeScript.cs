using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShakeScript : MonoBehaviour
{
    [Header("Testing Values")]
     [SerializeField] [Range(1f, 30f)] float shakeIntensity;
     [SerializeField] float shakeDuration;
     private float timer;
     public bool shakeScreen;
     private CinemachineVirtualCamera vCam;

     private void Awake(){
        vCam = GetComponent<CinemachineVirtualCamera>();
     }

    void Update()
    {
        if(shakeScreen){
            ShakeCam(shakeIntensity, shakeDuration);
            shakeScreen = false;
        }
        if(timer > 0){
            timer -= Time.deltaTime;
            if(timer <= 0f){
                CinemachineBasicMultiChannelPerlin noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                noise.m_AmplitudeGain = 0f;
            }
        }
    }

    public void ShakeCam(float intensity, float duration){
        CinemachineBasicMultiChannelPerlin noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = intensity;
        timer = duration;
    }
}
