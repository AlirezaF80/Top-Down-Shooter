using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamagable {
    public void Damage(int damageAmount) {
        print("Target damaged");
    }
}