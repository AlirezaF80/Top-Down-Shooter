using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisual : MonoBehaviour {
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private float muzzleFlashDuration = 0.1f;
    private float muzzleFlashTimer;

    private void Start() {
        Weapon.OnShoot += Weapon_OnShoot;
        muzzleTransform.gameObject.SetActive(false);
    }

    private void Weapon_OnShoot(object sender, EventArgs e) {
        muzzleFlashTimer = muzzleFlashDuration;
    }

    private void Update() {
        muzzleFlashTimer -= Time.deltaTime;
        muzzleTransform.gameObject.SetActive(muzzleFlashTimer > 0);
    }
}