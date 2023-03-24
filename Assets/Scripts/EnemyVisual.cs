using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour {
    private const string IS_WALKING = "IsWalking";
    private const string IS_HIT = "IsHit";

    [SerializeField] private Animator animator;
    [SerializeField] private bool hasAnimation;
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyAI enemyAI;

    private void Start() {
        enemy.OnHit += Enemy_OnHit;
    }


    private void Update() {
        if (hasAnimation)
            animator.SetBool(IS_WALKING, enemyAI.IsWalking());

        Vector3 aimDirection = enemyAI.GetAimDirection();
        if (aimDirection.x > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        } else if (aimDirection.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    private void Enemy_OnHit(object sender, EventArgs e) {
        if (hasAnimation)
            animator.SetTrigger(IS_HIT);
        CinemachineShake.Instance.ShakeCamera();
    }
}