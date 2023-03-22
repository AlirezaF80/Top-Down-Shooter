using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
    [SerializeField] private int healthAmount = 10;
    [SerializeField] private int timeToDeSpawn = 3;

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