using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IDamagable, IHealable {
    public static Player Instance { get; private set; }

    public event EventHandler<ShootEventArgs> OnShoot;
    public event EventHandler OnPlayerHit;
    public event EventHandler OnDash;

    public class ShootEventArgs : EventArgs {
        public Vector3 shootTarget;
    }

    private enum State {
        Moving,
        Dashing
    }


    [SerializeField] private Weapon weapon;
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
    private Rigidbody2D rb;

    private void Awake() {
        Instance = this;
        healthSystem = new HealthSystem(healthMax);
        rb = GetComponent<Rigidbody2D>();
        state = State.Moving;
    }

    private void Update() {
        HandleMovement();
        HandleShooting();
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

    private void HandleShooting() {
        if (Input.GetMouseButton(0)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnShoot?.Invoke(this, new ShootEventArgs { shootTarget = mousePos });
        }
    }

    private void HandleMovement() {
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (moveDir.magnitude > 0.1f) {
            lastMoveDir = moveDir.normalized;
        }

        switch (state) {
            case State.Moving:
                if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer < 0) {
                    dashTimer = dashTime;
                    dashCooldownTimer = dashCooldownMax;
                    dashDir = lastMoveDir;
                    state = State.Dashing;
                    OnDash?.Invoke(this, EventArgs.Empty);
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

    public void Damage(int damageAmount) {
        OnPlayerHit?.Invoke(this, EventArgs.Empty);
        healthSystem.Damage(damageAmount);
        Debug.Log("Player health: " + healthSystem.GetHealth());
    }

    public void Heal(int healAmount) {
        healthSystem.Heal(healAmount);
        Debug.Log("Player health: " + healthSystem.GetHealth());
    }

    public int GetHealth() {
        return healthSystem.GetHealth();
    }

    public int GetAmmo() {
        return weapon.GetAmmo();
    }

    public void AddAmmo(int ammoAmount) {
        weapon.AddAmmo(ammoAmount);
        Debug.Log("Player ammo: " + weapon.GetAmmo());
    }


    public float GetDashTime() {
        return dashTime;
    }

    public bool IsWalking() {
        float walkSpeedThreshold = 0.1f;
        return moveDir.magnitude > walkSpeedThreshold;
    }

    public int GetMaxHealth() {
        return healthMax;
    }
}