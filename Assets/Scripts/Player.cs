using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour {
    private enum State {
        Moving,
        Dashing
    }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTimeMax;
    [SerializeField] private float dashCooldownMax;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 moveDir;
    private Vector2 dashDir;
    private Vector2 lastMoveDir;

    private State state;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        state = State.Moving;
    }

    private void Update() {
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (moveDir.magnitude > 0.1f) {
            lastMoveDir = moveDir.normalized;
        }

        switch (state) {
            case State.Moving:
                if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer < 0) {
                    dashTimer = dashTimeMax;
                    dashCooldownTimer = dashCooldownMax;
                    dashDir = lastMoveDir;
                    state = State.Dashing;
                }

                break;
            case State.Dashing:
                dashTimer -= Time.deltaTime;
                if (dashTimer < 0) {
                    state = State.Moving;
                }

                break;
        }

        dashCooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate() {
        switch (state) {
            case State.Moving:
                rb.velocity = moveDir * moveSpeed;
                break;
            case State.Dashing:
                rb.velocity = dashDir * dashSpeed;
                break;
        }
    }
}