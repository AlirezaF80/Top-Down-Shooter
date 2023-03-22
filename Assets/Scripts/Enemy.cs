using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable {
    [SerializeField] private int healthMax = 100;
    private HealthSystem healthSystem;


    private void Awake() {
        healthSystem = new HealthSystem(healthMax);
    }

    public void Damage(int damageAmount) {
        healthSystem.Damage(damageAmount);
        if (healthSystem.GetHealth() == 0) {
            Destroy(gameObject);
        }
    }
}