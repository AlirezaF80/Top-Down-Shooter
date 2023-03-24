using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Vignette = UnityEngine.Rendering.Universal.Vignette;

public class LowHPVisual : MonoBehaviour {
    [SerializeField] private float lowHPThreshold = 25;
    [SerializeField] private float timeToFade = 2f;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private Volume volume;
    private Player player;

    private float curWeight;
    private float targetWeight;
    private float fadeTimer;
    private bool isLowHealth = false;

    private void Awake() {
        volume = GetComponent<Volume>();
        volume.weight = 0;
    }

    private void Start() {
        player = Player.Instance;
        volume.sharedProfile.TryGet<Vignette>(out vignette);
        volume.sharedProfile.TryGet<ChromaticAberration>(out chromaticAberration);
        volume.sharedProfile.TryGet<LensDistortion>(out lensDistortion);
    }

    private void Update() {
        if (player.GetHealth() < lowHPThreshold && !isLowHealth) {
            isLowHealth = true;
            targetWeight = 1;
            fadeTimer = timeToFade;
        } else if (player.GetHealth() >= lowHPThreshold && isLowHealth) {
            isLowHealth = false;
            targetWeight = 0;
            fadeTimer = timeToFade;
        }

        if (fadeTimer > 0) {
            fadeTimer -= Time.deltaTime;
            curWeight = Mathf.Lerp(curWeight, targetWeight, 1 - (fadeTimer / timeToFade));
            volume.weight = curWeight;
        }


        if (Math.Abs(targetWeight - 1) < 0.01) {
            // animate vignette and chromatic aberration to pulse
            vignette.intensity.value = Mathf.Lerp(0.23f, 0.33f, Mathf.PingPong(Time.time, 1));
            chromaticAberration.intensity.value = Mathf.Lerp(0.2f, 0.4f, Mathf.PingPong(Time.time, 1));
            lensDistortion.intensity.value = Mathf.Lerp(0.35f, 0.55f, Mathf.PingPong(Time.time, 1));
        }
    }
}