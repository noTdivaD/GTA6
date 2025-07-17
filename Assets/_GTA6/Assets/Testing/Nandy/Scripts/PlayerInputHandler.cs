using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMove;
    public static event Action OnJump;
    public static event Action OnDance;
    public static event Action<bool> OnSprint;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        inputActions.Player.Jump.performed += ctx => OnJump?.Invoke();
        inputActions.Player.Dance.performed += ctx => OnDance?.Invoke();

        inputActions.Player.Sprint.performed += ctx => OnSprint?.Invoke(true);
        inputActions.Player.Sprint.canceled += ctx => OnSprint?.Invoke(false);
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
}

