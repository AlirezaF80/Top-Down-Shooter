using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private int damageAmount;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 shootDir) {
        Vector2 bulletForce = shootDir * bulletSpeed;
        rb.AddForce(bulletForce, ForceMode2D.Impulse);
        Destroy(gameObject, bulletLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        IDamagable target = other.GetComponent<IDamagable>();
        if (target != null) {
            target.Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}