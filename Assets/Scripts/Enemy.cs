using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable {
    public static event EventHandler OnDeath;

    public class EnemyDeathEventArgs : EventArgs {
        public int EnemyMaxHealth { get; set; }
        public int DamageDealt { get; set; }
        public Vector3 DeathPosition { get; set; }
    }

    [SerializeField] private int healthMax;
    private HealthSystem healthSystem;


    private void Awake() {
        healthSystem = new HealthSystem(healthMax);
    }

    public void Damage(int damageAmount) {
        healthSystem.Damage(damageAmount);
        if (healthSystem.GetHealth() == 0) {
            OnDeath?.Invoke(this, new EnemyDeathEventArgs {
                EnemyMaxHealth = healthMax,
                DamageDealt = GetComponent<EnemyAI>().GetDamageDealt(),
                DeathPosition = transform.position
            });
            Destroy(gameObject);
        }
    }

    public int GetMaxHealth() {
        return healthMax;
    }
}