using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatsUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        EnemySpawner.OnWaveNumberChanged += EnemySpawner_OnWaveNumberChanged;
    }

    private void EnemySpawner_OnWaveNumberChanged(object sender, EventArgs e) {
        string waveNumber = EnemySpawner.Instance.GetWaveNumber().ToString();
        waveNumberText.text = "WAVE " + waveNumber;
    }

    private void Update() {
        scoreText.text = GameManager.Instance.GetScore().ToString();
    }
}