using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Fields
    private PlayerInput inputActions;
    
    // Emitted events
    // emits percent charge when fling is charging
    public static event Action<float> flingChargeEvent;

    void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Gameplay.Enable();
    }

    private void OnEnable() {
        inputActions.Gameplay.Claw.performed += OnClaw;
        inputActions.Gameplay.Fling.performed += OnStartFling;
        inputActions.Gameplay.Fling.canceled += OnFinishFling;
        inputActions.Gameplay.Jump.performed += OnJump;
        inputActions.Gameplay.Movement.performed += OnMovement;
    }

    private void OnDisable() {
        inputActions.Gameplay.Claw.performed -= OnClaw;
        inputActions.Gameplay.Fling.performed -= OnStartFling;
        inputActions.Gameplay.Fling.canceled -= OnFinishFling;
        inputActions.Gameplay.Jump.performed -= OnJump;
        inputActions.Gameplay.Movement.performed -= OnMovement;
    }

    private void OnClaw(InputAction.CallbackContext context) {
        Debug.Log("Claw");
    }

    private void OnStartFling(InputAction.CallbackContext context) {
        Debug.Log("Start Fling");
    }

    private void OnFinishFling(InputAction.CallbackContext context) {
        Debug.Log("Finish Fling");
    }
    
    private void OnJump(InputAction.CallbackContext context) {
        Debug.Log("Jump");
    }

    private void OnMovement(InputAction.CallbackContext context) {
        Vector2 movementVector = context.ReadValue<Vector2>();
        Debug.Log($"Movement Vector: {movementVector}");
    }
}
