using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable, IHealable {
    public static Player Instance { get; private set; }
    public event EventHandler OnDeath;
    public event EventHandler OnPlayerHit;
    public event EventHandler OnDash;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldownMax;
    [SerializeField] private int healthMax;
    
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 moveDir;
    private Vector2 dashDir;
    private Vector2 lastMoveDir;
    private HealthSystem healthSystem;
    private State state;

    private enum State {
        Moving,
        Dashing
    }

    private Rigidbody2D rb;


    private void Awake() {
        Instance = this;
        healthSystem = new HealthSystem(healthMax);
        rb = GetComponent<Rigidbody2D>();
        state = State.Moving;
    }

    private void Start() {
        InputSystemManager.Instance.OnDash += InputSystem_OnDash;
    }

    private void InputSystem_OnDash(object sender, EventArgs e) {
        if (dashCooldownTimer < 0) {
            dashDir = lastMoveDir;
            dashTimer = dashTime;
            dashCooldownTimer = dashCooldownMax;
            state = State.Dashing;
            OnDash?.Invoke(this, EventArgs.Empty);
        }
    }


    private void Update() {
        HandleMovement();
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


    private void HandleMovement() {
        moveDir = InputSystemManager.Instance.GetMoveInput();
        float moveSpeedThreshold = 0.1f;
        if (moveDir.magnitude > moveSpeedThreshold) {
            lastMoveDir = moveDir.normalized;
        }

        switch (state) {
            case State.Moving:
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

    public void Damage(int damageAmount) {
        OnPlayerHit?.Invoke(this, EventArgs.Empty);
        healthSystem.Damage(damageAmount);
        if (healthSystem.GetHealth() == 0) {
            Die();
        }
    }

    public void Heal(int healAmount) {
        healthSystem.Heal(healAmount);
    }

    public int GetHealth() {
        return healthSystem.GetHealth();
    }


    public float GetDashTime() {
        return dashTime;
    }

    public bool IsWalking() {
        float walkSpeedThreshold = 0.1f;
        return moveDir.magnitude > walkSpeedThreshold;
    }

    public void Die() {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }
}