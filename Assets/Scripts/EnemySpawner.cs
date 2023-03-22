using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private float spawnRadiusStart;
    [SerializeField] private float spawnRadiusEnd;
    [SerializeField] private int timeBetweenWaves = 5;

    private Dictionary<GameObject, int> enemyHealth = new();

    private int waveNumber = 1;
    private float spawnTimer;
    private int currentEnemiesHealth;

    private void Awake() {
        spawnTimer = 0;
        foreach (GameObject enemy in enemyPrefabs) {
            enemyHealth.Add(enemy, enemy.GetComponent<Enemy>().GetMaxHealth());
        }
    }

    private void Start() {
        Enemy.OnDeath += Enemy_OnDeath;
    }

    private void Enemy_OnDeath(object sender, EventArgs e) {
        Enemy.EnemyDeathEventArgs args = e as Enemy.EnemyDeathEventArgs;
        currentEnemiesHealth -= args.EnemyMaxHealth;
    }

    private void Update() {
        if (currentEnemiesHealth <= 0) {
            spawnTimer -= Time.deltaTime;
        }

        if (spawnTimer <= 0f) {
            SpawnWave(waveNumber);
            spawnTimer = timeBetweenWaves;
            waveNumber++;
        }
    }

    private void SpawnWave(int waveNumber) {
        currentEnemiesHealth = GetWaveHealth(waveNumber);
        Debug.Log("Wave " + waveNumber + " spawned with " + currentEnemiesHealth + " health");
        int enemiesSpawnedHealth = 0;

        while (enemiesSpawnedHealth < currentEnemiesHealth) {
            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            int randomEnemyHealth = enemyHealth[randomEnemy];

            Instantiate(randomEnemy, GetRandomSpawnPosition(), Quaternion.identity);
            enemiesSpawnedHealth += randomEnemyHealth;
        }
    }

    private Vector3 GetRandomSpawnPosition() {
        Vector3 randomDir = Random.insideUnitCircle.normalized;
        float randomRadius = Random.Range(spawnRadiusStart, spawnRadiusEnd);
        Vector3 spawnPos = transform.position + randomDir * randomRadius;
        return spawnPos;
    }

    private int GetWaveHealth(int wave) {
        return wave * 10;
    }
}