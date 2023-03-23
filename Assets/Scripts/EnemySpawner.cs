using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private List<GameObject> enemyPrefabList;
    [SerializeField] private float spawnRadiusStart;
    [SerializeField] private float spawnRadiusEnd;
    [SerializeField] private int timeBetweenWaves = 5;

    private Dictionary<GameObject, int> enemyHealthDict = new();
    [SerializeField] private List<int> waveToUnlockWeaponList = new();

    private int waveNumber = 0;
    private float spawnTimer;
    private int currentEnemiesHealth;
    private int currentWeaponIndex = 0;

    private void Awake() {
        spawnTimer = timeBetweenWaves;

        foreach (GameObject enemy in enemyPrefabList) {
            enemyHealthDict.Add(enemy, enemy.GetComponent<Enemy>().GetMaxHealth());
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
            waveNumber++;
            SpawnWave(waveNumber);
            spawnTimer = timeBetweenWaves;

            UnlockWeapon(waveNumber);
        }
    }

    private void SpawnWave(int waveNumber) {
        currentEnemiesHealth = 0;

        while (currentEnemiesHealth < GetWaveHealth(waveNumber)) {
            GameObject randomEnemy = enemyPrefabList[Random.Range(0, enemyPrefabList.Count)];
            int randomEnemyHealth = enemyHealthDict[randomEnemy];

            Instantiate(randomEnemy, GetRandomSpawnPosition(), Quaternion.identity);
            currentEnemiesHealth += randomEnemyHealth;
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

    private void UnlockWeapon(int waveNumber) {
        if (currentWeaponIndex >= waveToUnlockWeaponList.Count) return;
        if (waveNumber >= waveToUnlockWeaponList[currentWeaponIndex]) {
            WeaponManager.Instance.EquipWeapon(currentWeaponIndex);
            currentWeaponIndex++;
        }
    }
}