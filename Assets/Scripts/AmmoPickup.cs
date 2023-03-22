using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {
    [SerializeField] private int ammoCount = 10;
    [SerializeField] private int timeToDeSpawn = 3;

    public void Setup(int ammoCount) {
        this.ammoCount = ammoCount;
        Destroy(gameObject, timeToDeSpawn);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player != null) {
            player.AddAmmo(ammoCount);
            Destroy(gameObject);
        }
    }
}