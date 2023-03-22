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
    [SerializeField] private float timeBetweenAttacks = 2;
    private float damageTimer;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        ResetAttackTimer();
        state = State.Chasing;
    }

    private void ResetAttackTimer() {
        damageTimer = timeBetweenAttacks;
    }

    private void Start() {
        target = Player.Instance.transform;
    }

    private void Update() {
        if (!IsTargetInRange() && state == State.Attacking) {
            state = State.Chasing;
        } else if (IsTargetInRange() && state == State.Chasing) {
            ResetAttackTimer();
            state = State.Attacking;
            rb.velocity = Vector2.zero;
        }

        damageTimer -= Time.deltaTime;

        switch (state) {
            case State.Chasing:
                break;
            case State.Attacking:
                AttackTarget();
                break;
        }
    }

    private void FixedUpdate() {
        AimAtTarget();

        switch (state) {
            case State.Chasing:
                ChaseTarget();
                break;
            case State.Attacking:
                break;
        }
    }

    private void AimAtTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    private void AttackTarget() {
        if (damageTimer > 0) return;
        Player.Instance.Damage(damageAmount);
        ResetAttackTimer();
    }


    private void ChaseTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private bool IsTargetInRange() {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return distanceToTarget <= stopRadius;
    }
}