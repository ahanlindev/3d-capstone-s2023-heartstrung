using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Order is used by animator. If new states need to be added, append them
    private enum State { IDLE, MOVING, ATTACKING, CHARGING, FLINGING, DEAD };
    
    // editable fields
    [Tooltip("The heart that is connected to this player")]
    [SerializeField] private Heart heart;

    [Tooltip("Amount of time in seconds that the player will take when clawing")]
    [SerializeField] private float clawTime = 0.5f;

    [Tooltip("Player's max speed in units/second")]
    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("Player's jump power")]
    [SerializeField] private float jumpPower = 5f;

    [Tooltip("Player's max distance from Dodger in units")]
    [SerializeField] private float maxTetherLength = 3f;

    [Tooltip("Percentage of fling power that will fill or lessen per second charging a fling")]
    [Range(0f,1f)] [SerializeField] private float powerPerSecond = 0.85f;

    // private variables
    // source of input events
    private PlayerInput inputActions;

    // current player state
    private State currentState;

    // player's rigidbody component
    private Rigidbody rbody;

    // player's collider component
    private Collider coll;

    // player's animator component
    private Animator anim;
    
    // used when charging: if true, fling power will increase next tick. Else, false
    private bool increasingPower;
    private float currentFlingPower;

    // Emitted events
    // informs listeners that the player has started charging their fling
    public static event Action flingStartEvent;
    // emits percent fling power when fling is charging
    public static event Action<float> flingPowerChangeEvent;
    // informs listeners that the player is flinging. Parameter is final power percentage
    public static event Action<float> flingEvent;

    // Setup/Teardown methods
    void Awake()
    {
        // initialize input
        inputActions = new PlayerInput();
        inputActions.Gameplay.Enable();

        // fill fields
        currentState = State.IDLE;
        rbody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        // validate inspector-filled values
        if (!heart) { Debug.LogError("Player script has no Heart set!"); }
    }

    void Start() {
        // initialize the length of the tether
        var joint = GetComponent<ConfigurableJoint>();
        var limit = joint.linearLimit;
        limit.limit = maxTetherLength;
        joint.linearLimit = limit;
    }

    private void OnEnable() {
        inputActions.Gameplay.Claw.performed += OnPlayerClaw;
        inputActions.Gameplay.Fling.performed += OnStartFling;
        inputActions.Gameplay.Fling.canceled += OnFinishFling;
        inputActions.Gameplay.Jump.performed += OnPlayerJump;

        Heart.LandedEvent += OnHeartLanded;
    }

    private void OnDisable() {
        inputActions.Gameplay.Claw.performed -= OnPlayerClaw;
        inputActions.Gameplay.Fling.performed -= OnStartFling;
        inputActions.Gameplay.Fling.canceled -= OnFinishFling;
        inputActions.Gameplay.Jump.performed -= OnPlayerJump;
    }

    // Update functions

    private void FixedUpdate() {
        // Movement needs to be done here because this is the best way to get continuous input
        var moveVect = inputActions.Gameplay.Movement.ReadValue<Vector2>();
        
        // if player can move, move them
        if (moveVect != Vector2.zero && TryChangeState(State.MOVING)) {
            DoMovement(moveVect);
        }
    }

    private void Update() {
        // update fling power and inform any other listening scripts
        if (currentState == State.CHARGING) { 
            UpdateFlingPower();
            flingPowerChangeEvent?.Invoke(currentFlingPower);
        }
        UpdateAnimationParams();
    }

    /// <summary>
    /// Updates the percentage fling power strength based on current power 
    /// and whether it is set to increase or decrease
    ///</summary>
    private void UpdateFlingPower() {
        // update power amount
        float powerDelta = powerPerSecond * Time.deltaTime;
        currentFlingPower += (increasingPower) ? powerDelta : -powerDelta;
        
        // flip power delta if endpoints exceeded
        if (currentFlingPower <= 0.0f) { increasingPower = true; }
        else if (currentFlingPower >= 1.0f) { increasingPower = false; }

        // clamp power within appropriate levels
        currentFlingPower = Mathf.Clamp(currentFlingPower, 0f, 1f);
    }

    // Input handlers

    // For some reason, throws an error if name is "DoClaw"
    /// <summary>If the player is able to claw, does so</summary>
    /// <param name="_">Input context. Unused.</param>
    private void OnPlayerClaw(InputAction.CallbackContext context) {
        if (!TryChangeState(State.ATTACKING)) { return; }
        StartCoroutine(ClawTimer());
    }

    // TODO: this timer is a sloppy way of deciding how long an attack lasts. Should figure out based on animation itself
    private IEnumerator ClawTimer() {
        yield return new WaitForSeconds(clawTime);
        TryChangeState(State.IDLE);
    }

    /// <summary>If the player is able to start charging a fling, does so.</summary>
    /// <param name="_">Input context. Unused.</param>
    private void OnStartFling(InputAction.CallbackContext _) {
        if (!TryChangeState(State.CHARGING)) { return; }

        // reset fling power
        currentFlingPower = 0.0f;

        // face player directly away from the heart
        Vector3 vecToHeart = heart.transform.position - transform.position;
        vecToHeart.y = 0.0f; // don't want to rotate player too much
        rbody.MoveRotation(Quaternion.LookRotation(-vecToHeart, transform.up));

        flingStartEvent?.Invoke();
    }
    
    /// <summary>If the player is charging a fling, executes the fling.</summary>
    /// <param name="_">Input context. Unused.</param>
    private void OnFinishFling(InputAction.CallbackContext _) {
        if (currentState != State.CHARGING) { return; }

        flingEvent?.Invoke(currentFlingPower);

        TryChangeState(State.FLINGING);
    }
    
    /// <summary>If the player is able to jump, applies an upward physics impulse.</summary>
    /// <param name="_">Input context. Unused.</param>
    private void OnPlayerJump(InputAction.CallbackContext _) {
        if (!TryChangeState(State.MOVING) || !IsGrounded()) { return; }
        anim.SetTrigger("JumpStart");
        Vector3 jumpVec = transform.up * jumpPower;
        rbody.AddForce(jumpVec, ForceMode.Impulse);
    }

    /// <summary>Performs per-physics-tick movement based on player movement input</summary>
    /// <param name="moveInput">raw 2-axis input vector</param>
    private void DoMovement(Vector2 moveInput) {       
        // early-return if needed
        if (moveInput == Vector2.zero) { return; }
        var moveInput3D = new Vector3(moveInput.x, 0.0f, moveInput.y);

        // account for player move speed and tick rate
        moveInput3D *= moveSpeed;
        moveInput3D *= Time.fixedDeltaTime;

        // find proper positon and look rotation
        var newPos = transform.position + moveInput3D;
        var newRot = Quaternion.LookRotation(moveInput3D.normalized, transform.up);

        // set rigidbody position and rotation accordingly
        rbody.MovePosition(newPos);
        rbody.MoveRotation(newRot);
    }
    
    // Other Event Handlers

    /// <summary>
    /// Attempts to go to Idle state when the heart lands. 
    /// This should take the player out of Flinging state.
    /// </summary>
    private void OnHeartLanded() {
        TryChangeState(State.IDLE);
    }

    // State management

    /// <summary>Check if the current state has a valid transition to the desired new state.</summary>
    /// <param name="newState">the desired new state.</param>
    /// <returns>true if transition to newState will work, false otherwise.</returns>
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
                // player can't attack or start moving while charging
                result = (newState != State.ATTACKING && newState != State.MOVING);
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
        
        // special case if heart is in a non-flingable state
        if ((newState == State.CHARGING || newState == State.FLINGING) 
                && !heart.CanBeFlung()) {
            result = false;
        }
        return result;
    }

    /// <summary>Attempts to change the player's current state to the parameter.</summary>
    /// <param name="newState">the state attempting to replace the current state.</param>
    /// <returns>true if change was successful, false otherwise.</returns>
    private bool TryChangeState(State newState) {
        if (!CanChangeState(newState)) { return false; }

        currentState = newState;
        return true;
    }

    /// <summary>Checks if player is grounded using a raycast.</summary>
    /// <returns>True if the player is grounded, otherwise false.</returns>
    // TODO currently can fail on edges of platform. Add coyote time.
    private bool IsGrounded() {
        // send raycast straight downward. If it hits nothing, the player must be airborne.
        float distToGround = coll.bounds.extents.y;
        return Physics.Raycast(transform.position, -transform.up, distToGround + 0.1f);
    }

    /// <summary>Updates each animation param that may change from frame to frame.</summary>
    private void UpdateAnimationParams() {
        anim.SetBool("Grounded", IsGrounded());
        anim.SetInteger("State", (int)currentState);
    }
}
