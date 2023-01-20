using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum State { IDLE, MOVING, ATTACKING, CHARGING, FLINGING, DEAD };
    
    
    // editable fields
    [Tooltip("Amount of time in seconds that the player will take when clawing")]
    [SerializeField] private float clawTime = 0.5f;

    [Tooltip("Player's max speed in units/second")]
    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("Player's jump power")]
    [SerializeField] private float jumpPower = 5f;

    [Tooltip("Player's max distance from Dodger in units")]
    [SerializeField] private float maxTetherLength = 3f;

    // private variables
    private PlayerInput inputActions;
    private State currentState;
    private Rigidbody rbody;
    
    // Emitted events
    // emits percent charge when fling is charging
    public static event Action<float> flingChargeEvent;

    // Setup/Teardown methods
    void Awake()
    {
        // initialize input
        inputActions = new PlayerInput();
        inputActions.Gameplay.Enable();

        // fill fields
        currentState = State.IDLE;
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

    // Input Event handlers

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

    // State management
    private bool CanChangeState(State newState) {
        // result may change depending on combination of current and new state
        bool result = false;

        switch (currentState) {
            case State.IDLE:
                // player can go from idle to any other state
                result = true; 
                break;
            case State.MOVING:
                // player can go from moving to any other state
                result = true; 
                break;
            case State.ATTACKING:
                // player can't exit out of an attack by moving or flinging
                result = (newState != State.MOVING && newState != State.FLINGING);
                break;
            case State.CHARGING:
                // player can't attack while charging
                result = (newState != State.ATTACKING);
                break;
            case State.FLINGING:
                // player can't start attacking or moving while attempting to fling
                result = (newState != State.MOVING && newState != State.ATTACKING);
                break;
            case State.DEAD:
                // if the player is dead, they can't change state
                result = false;
                break;
            default:
                Debug.LogError("Unhandled Player State encountered!");
                break;
        }

        return result;
    }

    /// <summary>Attempts to change the player's current state to the parameter.</summary>
    /// <param name="newState">the state attempting to replace the current state.</param>
    /// <returns>true if change was successful, false otherwise.</returns>
    private bool TryChangeState(State newState) {
        if (!CanChangeState(newState)) { return false; }

        currentState = newState;
        InitState(newState);
        return true;
    }

    private void InitState(State newState) {
        switch (newState) {
            case State.IDLE:
                break;
            case State.MOVING:
                break;
            case State.CHARGING:
                break;
            case State.FLINGING:
                break;
            case State.ATTACKING:
                break;
            case State.DEAD:
                break;
            default:
                Debug.LogError("Unhandled Player State encountered!");
                break;
        }
    }
}
