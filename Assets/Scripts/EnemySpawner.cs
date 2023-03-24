using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    public static EnemySpawner Instance { get; private set; }
    public static event EventHandler OnWaveNumberChanged;
    [SerializeField] private List<GameObject> enemyPrefabList;
    [SerializeField] private float spawnRadiusStart;
    [SerializeField] private float spawnRadiusEnd;
    [SerializeField] private int timeBetweenWaves = 5;
    [SerializeField] private List<int> waveToUnlockWeaponList = new();

    private Dictionary<GameObject, int> enemyHealthDict = new();

    private int waveNumber = 0;
    private float spawnTimer;
    private int currentEnemiesHealth;
    private int nextWeaponIndex = 0;

    private void Awake() {
        Instance = this;
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
        
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
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
        if (nextWeaponIndex >= waveToUnlockWeaponList.Count) return;
        if (waveNumber >= waveToUnlockWeaponList[nextWeaponIndex]) {
            WeaponManager.Instance.EquipWeapon(nextWeaponIndex);
            nextWeaponIndex++;
        }
    }
    
    public int GetWaveNumber() {
        return waveNumber;
    }
}