using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private float shootRate;
    [SerializeField] private int initialAmmo;
    [SerializeField] private GameObject bulletPrefab;

    private float shootTimer;
    private int ammo;

    private void Start() {
        Player.Instance.OnShoot += Player_OnShoot;
        shootTimer = shootRate;
        ammo = initialAmmo;
    }

    private void Update() {
        shootTimer -= Time.deltaTime;
    }

    private void Player_OnShoot(object sender, Player.ShootEventArgs e) {
        Shoot(e.shootTarget);
    }

    private void Shoot(Vector3 shootTarget) {
        if (ammo <= 0) return;
        if (shootTimer > 0) return;
        shootTimer = shootRate;
        ammo--;

        Vector2 shootDir = (shootTarget - transform.position).normalized;
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Setup(shootDir);
    }

    public void AddAmmo(int ammoAmount) {
        ammo += ammoAmount;
    }

    public int GetAmmo() {
        return ammo;
    }
}