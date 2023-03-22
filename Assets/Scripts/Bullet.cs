using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rb;
    float bulletLifeTime;
    private int damageAmount;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 shootDir, float bulletSpeed, float bulletMaxDistance, int damageAmount) {
        this.damageAmount = damageAmount;
        
        float bulletLifeTime = bulletMaxDistance / bulletSpeed;
        
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg);
        
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