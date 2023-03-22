using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletMaxDistance;
    [SerializeField] private int bulletDamageAmount;
    [SerializeField] private int bulletPerShot;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private int initialAmmo;
    [SerializeField] private int spreadAngle;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunEndPoint;

    private float shootTimer;
    private int ammo;

    private void Start() {
        Player.Instance.OnShoot += Player_OnShoot;
        ResetShootTimer();
        ammo = initialAmmo;
    }

    private void ResetShootTimer() {
        shootTimer = timeBetweenShots;
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
        ResetShootTimer();

        for (int i = 0; i < Mathf.Min(bulletPerShot, ammo); i++) {
            ShootBullet(shootTarget);
        }

        ammo = Mathf.Max(0, ammo - bulletPerShot);
    }

    private void ShootBullet(Vector3 shootTarget) {
        Vector2 shootDir = (shootTarget - gunEndPoint.position).normalized;
        float randomSpreadAngle = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
        shootDir = Quaternion.Euler(0, 0, randomSpreadAngle) * shootDir;
        GameObject bulletGO = Instantiate(bulletPrefab, gunEndPoint.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Setup(shootDir, bulletSpeed, bulletMaxDistance, bulletDamageAmount);
    }

    public void AddAmmo(int ammoAmount) {
        ammo += ammoAmount;
    }

    public int GetAmmo() {
        return ammo;
    }
}