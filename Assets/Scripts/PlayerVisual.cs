using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    private const string IS_WALKING = "IsWalking";
    private const string IS_HIT = "IsHit";

    [SerializeField] private Animator animator;
    [SerializeField] private TrailRenderer trailRenderer;
    private Player player;

    private float dashTime;
    private float dashTimer;


    private void Start() {
        player = Player.Instance;
        trailRenderer.time = player.GetDashTime();
        trailRenderer.enabled = false;
        dashTime = player.GetDashTime();

        player.OnPlayerHit += Player_OnPlayerHit;
        player.OnDash += Player_OnDash;
    }


    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0) {
            trailRenderer.enabled = false;
        }
    }

    private void Player_OnDash(object sender, EventArgs e) {
        trailRenderer.enabled = true;
        trailRenderer.Clear();
        dashTimer = dashTime;
    }

    private void Player_OnPlayerHit(object sender, EventArgs e) {
        animator.SetTrigger(IS_HIT);
    }
}