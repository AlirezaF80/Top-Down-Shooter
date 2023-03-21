using UnityEngine;

public class HealthSystem {
    private int healthMax;
    private int health;

    public HealthSystem(int healthMax) {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth() {
        return health;
    }

    public float GetHealthPercent() {
        return (float)health / healthMax;
    }

    public void Damage(int damageAmount) {
        health = Mathf.Clamp(health - damageAmount, 0, healthMax);
    }

    public void Heal(int healAmount) {
        health += healAmount;
        health = Mathf.Clamp(health + healAmount, 0, healthMax);
    }
}