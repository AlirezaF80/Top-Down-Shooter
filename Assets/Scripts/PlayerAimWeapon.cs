using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour {
    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform playerVisualTransform;

    private void Update() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90) {
            localScale.y = -1f;
        } else {
            localScale.y = 1f;
        }
        aimTransform.localScale = localScale;
        localScale = Vector3.one;
        if (angle > 90 || angle < -90) {
            localScale.x = -1f;
        } else {
            localScale.x = 1f;
        }
        playerVisualTransform.localScale = localScale;
    }
}