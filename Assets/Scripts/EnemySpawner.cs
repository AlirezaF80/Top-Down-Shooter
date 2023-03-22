using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 5;
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private float spawnRadiusStart;
    [SerializeField] private float spawnRadiusEnd;

    private float spawnTimer;
    private int enemiesSpawned;

    private void Start() {
        spawnTimer = spawnDelay;
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && enemiesSpawned < enemyCount) {
            SpawnEnemy();
            spawnTimer = spawnDelay;
        }
    }

    private void SpawnEnemy() {
        Vector3 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        float randomRadius = UnityEngine.Random.Range(spawnRadiusStart, spawnRadiusEnd);
        Vector3 spawnPos = transform.position + randomDir * randomRadius;
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemiesSpawned++;
    }
}