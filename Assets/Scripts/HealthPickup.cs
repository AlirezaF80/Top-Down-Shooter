using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
    [SerializeField] private int defaultHealthAmount = 10;
    [SerializeField] private int timeToDeSpawn = 5;
    private int healthAmount;
    private void Start() {
        healthAmount = defaultHealthAmount;
    }

    public void Setup(int healthAmount) {
        this.healthAmount = healthAmount;
        Destroy(gameObject, timeToDeSpawn);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player != null) {
            player.Heal(healthAmount);
            Destroy(gameObject);
        }
    }
}