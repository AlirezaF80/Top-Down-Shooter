using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    public static WeaponManager Instance { get; private set; }
    public static event EventHandler OnWeaponUnlocked;

    [SerializeField] private List<GameObject> weapons;
    private Weapon currentWeapon;

    private void Awake() {
        Instance = this;
    }

    public void AddAmmo(int ammoAmount) {
        currentWeapon.AddAmmo(ammoAmount);
    }

    public int GetAmmo() {
        return currentWeapon.GetAmmo();
    }

    public void EquipWeapon(int weaponIndex) {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count) return;
        for (int i = 0; i < weapons.Count; i++) {
            weapons[i].SetActive(i == weaponIndex);
        }

        currentWeapon = weapons[weaponIndex].GetComponent<Weapon>();
        OnWeaponUnlocked?.Invoke(this, EventArgs.Empty);
    }

    public bool HasWeapon() {
        return currentWeapon != null;
    }
}