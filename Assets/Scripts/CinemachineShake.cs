using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour {
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float shakeIntensity = 5f;
    [SerializeField] private float shakeDuration = 0.1f;
    private float shakeTimer;

    private void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start() {
        Enemy.OnHit += Enemy_OnHit;
    }

    private void Enemy_OnHit(object sender, EventArgs e) {
        ShakeCamera();
    }

    public void ShakeCamera() {
        CinemachineBasicMultiChannelPerlin virtualCameraNoise =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        virtualCameraNoise.m_AmplitudeGain = shakeIntensity;
        shakeTimer = shakeDuration;
    }

    private void Update() {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin virtualCameraNoise =
                virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            virtualCameraNoise.m_AmplitudeGain = Mathf.Lerp(shakeIntensity, 0f, 1 - (shakeTimer / shakeDuration));
        }
    }
}