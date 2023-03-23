using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {
    [SerializeField] private GameObject healthPickup;
    [SerializeField] private GameObject ammoPickup;
    [SerializeField] private int timeBetweenPickups = 5;
    [SerializeField] private int spawnRadius = 15;
    [SerializeField] private int minAmmoBeforeSpawn = 25;
    [SerializeField] private int minHealthBeforeSpawn = 30;
    [SerializeField] private int ammoPickupCount = 15;
    [SerializeField] private int healthPickupAmount = 15;

    private float pickupTimer;
    private Player player;

    private void Start() {
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
            if (args.DamageDealt > 0)
                SpawnHealthPickup(args.DamageDealt, args.DeathPosition);
        } else { // 33% chance to do nothing
            return;
        }
    }

    private void SpawnPickup() {
        Vector3 randomSpawnPosition = GetRandomSpawnPosition();
        if (player.GetHealth() < minHealthBeforeSpawn) {
            SpawnHealthPickup(healthPickupAmount, randomSpawnPosition);
            Debug.Log("Spawned health pickup");
        }

        randomSpawnPosition = GetRandomSpawnPosition();
        if (player.GetAmmo() < minAmmoBeforeSpawn) {
            SpawnAmmoPickup(ammoPickupCount, randomSpawnPosition);
            Debug.Log("Spawned ammo pickup");
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