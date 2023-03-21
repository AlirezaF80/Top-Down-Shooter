using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 shootDir, float bulletSpeed, float bulletLifeTime) {
        Vector2 bulletForce = shootDir * bulletSpeed;
        rb.AddForce(bulletForce, ForceMode2D.Impulse);
        Destroy(gameObject, bulletLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Target target = other.GetComponent<Target>();
        if (target != null) {
            target.Damage();
            Destroy(gameObject);
        }
    }
}