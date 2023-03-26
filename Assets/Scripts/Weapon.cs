using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public static event EventHandler OnShoot;

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
        ResetShootTimer();
        ammo = initialAmmo;
    }

    private void Update() {
        shootTimer -= Time.deltaTime;
        if (InputSystemManager.Instance.IsShooting()) {
            Vector3 shootTarget = InputSystemManager.Instance.GetMouseWorldPosition();
            Shoot(shootTarget);
        }
    }

    private void ResetShootTimer() {
        shootTimer = timeBetweenShots;
    }


    private void Shoot(Vector3 shootTarget) {
        if (ammo <= 0) return;
        if (shootTimer > 0) return;
        ResetShootTimer();

        int numBulletsToShoot = Mathf.Min(bulletPerShot, ammo);
        for (int i = 0; i < numBulletsToShoot; i++) {
            ShootBullet(shootTarget, -spreadAngle / 2 + i * spreadAngle / (numBulletsToShoot));
        }

        OnShoot?.Invoke(this, EventArgs.Empty);
        ammo = Mathf.Max(0, ammo - bulletPerShot);
    }

    private void ShootBullet(Vector3 shootTarget, float bulletSpreadAngle) {
        Vector2 shootDir = (shootTarget - gunEndPoint.position).normalized;
        shootDir = Quaternion.Euler(0, 0, bulletSpreadAngle) * shootDir;
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