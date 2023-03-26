using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }


    private int waveNumber;
    private int score;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Enemy.OnDeath += Enemy_OnDeath;
        InputSystemManager.Instance.OnRestart += InputSystem_OnRestart;
        Player.Instance.OnDeath += Player_OnDeath;
    }

    private void Player_OnDeath(object sender, EventArgs e) {
        Restart();
    }

    private void InputSystem_OnRestart(object sender, EventArgs e) {
        Restart();
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Enemy_OnDeath(object sender, Enemy.EnemyDeathEventArgs e) {
        AddScore(Mathf.Max(e.EnemyMaxHealth - e.DamageDealt, 0));
    }

    public void AddScore(int score) {
        this.score += score;
    }


    public int GetScore() {
        return score;
    }
}