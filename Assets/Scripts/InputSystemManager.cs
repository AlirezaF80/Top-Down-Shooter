using System;
using UnityEngine;

public class InputSystemManager : MonoBehaviour {
    public static InputSystemManager Instance { get; private set; }

    public event EventHandler OnDash;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            OnDash?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsShooting() {
        return Input.GetButton("Fire1");
    }

    public Vector2 GetMouseWorldPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2 GetMoveInput() {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}