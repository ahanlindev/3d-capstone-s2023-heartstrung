using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
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
        rbody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        inputActions.Gameplay.Claw.performed += OnPlayerClaw;
        inputActions.Gameplay.Fling.performed += OnStartFling;
        inputActions.Gameplay.Fling.canceled += OnFinishFling;
        inputActions.Gameplay.Jump.performed += OnPlayerJump;
    }

    private void OnDisable() {
        inputActions.Gameplay.Claw.performed -= OnPlayerClaw;
        inputActions.Gameplay.Fling.performed -= OnStartFling;
        inputActions.Gameplay.Fling.canceled -= OnFinishFling;
        inputActions.Gameplay.Jump.performed -= OnPlayerJump;
    }

    // per-frame updates
    private void FixedUpdate() {
        // Movement needs to be done here because this is the best way to get continuous input
        var moveVect = inputActions.Gameplay.Movement.ReadValue<Vector2>() * Time.fixedDeltaTime;
        
        // if player can move, move them
        if (moveVect != Vector2.zero && TryChangeState(State.MOVING)) {
                DoMovement(moveVect);
        }
    }

    // Input Event handlers

    // For some reason, throws an error if name it "DoClaw"
    private void OnPlayerClaw(InputAction.CallbackContext context) {
        Debug.Log("Clawing!");
    }

    private void OnStartFling(InputAction.CallbackContext context) {
        Debug.Log("Charging Fling!");
    }

    private void OnFinishFling(InputAction.CallbackContext context) {
        Debug.Log("Flinging!");
    }
    
    private void OnPlayerJump(InputAction.CallbackContext context) {
        Debug.Log("Jumping!");
    }

    private void DoMovement(Vector2 moveInput) {       
        // early-return if needed
        if (moveInput == Vector2.zero) { return; }
        
        // account for player move speed
        moveInput *= moveSpeed;

        // find proper positon and look rotation
        var newPos = transform.position + new Vector3(moveInput.x, 0.0f, moveInput.y);
        var newRot = Quaternion.LookRotation(newPos, transform.up);

        // set rigidbody position and rotation accordingly
        rbody.MovePosition(newPos);
        rbody.MoveRotation(newRot);
        Debug.Log($"Movement Vector: {moveInput}");
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
        Debug.Log($"TryChangeState from {currentState} to {newState} result: {result}");
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
