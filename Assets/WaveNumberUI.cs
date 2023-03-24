using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveNumberUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI waveNumberText;

    private void Start() {
        EnemySpawner.OnWaveNumberChanged += EnemySpawner_OnWaveNumberChanged;
    }

    private void EnemySpawner_OnWaveNumberChanged(object sender, EventArgs e) {
        string waveNumber = EnemySpawner.Instance.GetWaveNumber().ToString();
        waveNumberText.text = "WAVE " + waveNumber;
    }
}