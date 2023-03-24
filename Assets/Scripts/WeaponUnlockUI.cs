using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnlockUI : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInDuration = 2f;
    [SerializeField] private float fadeOutDuration = 2f;
    [SerializeField] private float visibleDuration = 2f;

    private float timer = 0f;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private bool isVisible = false;

    private void OnEnable() {
        WeaponManager.OnWeaponUnlocked += StartFadeIn;
    }

    private void OnDisable() {
        WeaponManager.OnWeaponUnlocked -= StartFadeIn;
    }

    private void StartFadeIn(object sender, EventArgs args) {
        isFadingIn = true;
        isFadingOut = false;
        isVisible = false;
        canvasGroup.alpha = 0f;
    }

    private void Update() {
        timer += Time.deltaTime;

        if (isFadingIn) {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            canvasGroup.alpha = alpha;

            if (timer >= fadeInDuration) {
                isFadingIn = false;
                isVisible = true;
                timer = 0f;
            }
        } else if (isFadingOut) {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
            canvasGroup.alpha = alpha;

            if (timer >= fadeOutDuration) {
                isFadingOut = false;
                canvasGroup.alpha = 0f;
                timer = 0f;
            }
        } else if (isVisible) {
            if (timer >= visibleDuration) {
                isVisible = false;
                isFadingOut = true;
                timer = 0f;
            }
        }
    }
}