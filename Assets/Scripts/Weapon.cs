using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private float shootRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private GameObject bulletPrefab;

    float shootTimer;

    private void Start() {
        Player.Instance.OnShoot += Player_OnShoot;
        shootTimer = shootRate;
    }

    private void Update() {
        shootTimer -= Time.deltaTime;
    }

    private void Player_OnShoot(object sender, Player.ShootEventArgs e) {
        Shoot(e.shootTarget);
    }

    private void Shoot(Vector3 shootTarget) {
        if (shootTimer > 0) return;
        shootTimer = shootRate;

        Vector2 shootDir = (shootTarget - transform.position).normalized;
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Setup(shootDir, bulletSpeed, bulletLifeTime);
    }
}