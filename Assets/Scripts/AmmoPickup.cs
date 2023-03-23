using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {
    [SerializeField] private int defaultAmmoCount = 10;
    [SerializeField] private int timeToDeSpawn = 5;
    private int ammoCount;

    private void Start() {
        ammoCount = defaultAmmoCount;
    }

    public void Setup(int ammoCount) {
        this.ammoCount = ammoCount;
        Destroy(gameObject, timeToDeSpawn);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        WeaponManager weaponManager = other.GetComponent<WeaponManager>();
        if (weaponManager != null) {
            WeaponManager.Instance.AddAmmo(ammoCount);
            Destroy(gameObject);
        }
    }
}