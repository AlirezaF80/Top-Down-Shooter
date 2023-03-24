using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {
    [SerializeField] private GameObject healthPickup;
    [SerializeField] private GameObject ammoPickup;
    [SerializeField] private int timeBetweenPickups = 3;
    [SerializeField] private int spawnRadius = 10;
    [SerializeField] private int minAmmoBeforeSpawn = 25;
    [SerializeField] private int minHealthBeforeSpawn = 35;
    [SerializeField] private int ammoPickupCount = 10;
    [SerializeField] private int healthPickupAmount = 10;

    private float pickupTimer;
    private WeaponManager weaponManager;
    private Player player;

    private void Start() {
        weaponManager = WeaponManager.Instance;
        pickupTimer = timeBetweenPickups;

        player = Player.Instance;
        Enemy.OnDeath += Enemy_OnDeath;
    }

    private void Update() {
        pickupTimer -= Time.deltaTime;
        if (pickupTimer <= 0f) {
            SpawnPickup();
            pickupTimer = timeBetweenPickups;
        }
    }

    private void Enemy_OnDeath(object sender, EventArgs e) {
        Enemy.EnemyDeathEventArgs args = e as Enemy.EnemyDeathEventArgs;
        int random = UnityEngine.Random.Range(0, 3);
        if (random == 0) { // 33% chance to spawn ammo
            SpawnAmmoPickup(args.EnemyMaxHealth * 2, args.DeathPosition);
        } else if (random == 1) { // 33% chance to spawn health
            if (args.DamageDealt > 0) {
                int healthAmount = UnityEngine.Random.Range(args.DamageDealt / 4, args.DamageDealt);
                SpawnHealthPickup(healthAmount, args.DeathPosition);
            }
        } else { // 33% chance to do nothing
            return;
        }
    }

    private void SpawnPickup() {
        Vector3 randomSpawnPosition = GetRandomSpawnPosition();
        if (player.GetHealth() < minHealthBeforeSpawn) {
            SpawnHealthPickup(healthPickupAmount, randomSpawnPosition);
        }

        randomSpawnPosition = GetRandomSpawnPosition();
        if (weaponManager.HasWeapon())
            if (weaponManager.GetAmmo() < minAmmoBeforeSpawn) {
                SpawnAmmoPickup(ammoPickupCount, randomSpawnPosition);
            }
    }

    private Vector3 GetRandomSpawnPosition() {
        Vector3 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        return transform.position + randomDir * spawnRadius;
    }

    private void SpawnAmmoPickup(int ammoCount, Vector3 position) {
        Instantiate(ammoPickup, position, Quaternion.identity).GetComponent<AmmoPickup>().Setup(ammoCount);
    }

    private void SpawnHealthPickup(int healthAmount, Vector3 position) {
        Instantiate(healthPickup, position, Quaternion.identity).GetComponent<HealthPickup>().Setup(healthAmount);
    }
}