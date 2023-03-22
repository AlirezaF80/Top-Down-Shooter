using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    private enum State {
        Chasing,
        Attacking
    }

    // a simple script to make the enemy move towards the player
    // and damage the player when it collides with it
    private Transform target;
    private State state = State.Chasing;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopRadius = 1f;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float damageRate = 2;
    private float damageTimer;

    private void Start() {
        target = Player.Instance.transform;
        state = State.Chasing;
        damageTimer = damageRate;
    }

    private void Update() {
        if (!IsTargetInRange() && state == State.Attacking) {
            state = State.Chasing;
        } else if (IsTargetInRange() && state == State.Chasing) {
            damageTimer = damageRate;
            state = State.Attacking;
        }

        damageTimer -= Time.deltaTime;

        switch (state) {
            case State.Chasing:
                ChaseTarget();
                break;
            case State.Attacking:
                AttackTarget();
                break;
        }
    }


    private void AttackTarget() {
        if (damageTimer > 0) return;
        Player.Instance.Damage(damageAmount);
        damageTimer = damageRate;
    }


    private void ChaseTarget() {
        Vector3 targetPosition = target.position;
        Vector3 curPosition = transform.position;
        Vector3 dir = (targetPosition - curPosition).normalized;
        Vector3 newPosition = curPosition + dir * (moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private bool IsTargetInRange() {
        Vector3 targetPosition = target.position;
        Vector3 curPosition = transform.position;
        float distanceToTarget = Vector3.Distance(curPosition, targetPosition);
        return distanceToTarget <= stopRadius;
    }
}