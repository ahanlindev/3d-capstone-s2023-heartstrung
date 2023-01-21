using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Heart : MonoBehaviour
{
    private enum State { IDLE, FLUNG, FALLING, HURT, DEAD };

    // inspector fields
    [Tooltip("PlayerController instance for the player attached to this object")]
    [SerializeField] private PlayerController player;
    
    // private fields
    // rigidbody of the heart
    private Rigidbody rbody;
    
    // collider of the heart
    private Collider coll;
    
    // true when the heart is actively being flung. 
    [SerializeField] private State currentState;


    // Emitted events
    public static event Action LandedEvent;

    // Setup/Teardown
    private void Awake() {
        if (!player) { Debug.LogError("Heart script has no Player set!"); }

        rbody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    private void OnEnable() {
        PlayerController.flingEvent += OnPlayerFling;
    }

    private void OnDisable() {
        PlayerController.flingEvent -= OnPlayerFling;
    }

    // Accessors

    public bool CanBeFlung() => CanChangeState(State.FLUNG);

    // Event Handlers

    /// <summary>Subscribes to the player's fling event. Will fling this object around the player</summary>
    void OnPlayerFling(float power) {
        Debug.Log($"Flung by player with power {power}");
        //DEBUG
        StartCoroutine(DEBUGFlingTimer());
    }

    private void OnCollisionEnter(Collision other) {
        // hit wall or floor
        if (currentState == State.FLUNG) {
            if (IsGrounded()) {
                TryChangeState(State.IDLE);
            } else {
                TryChangeState(State.FALLING);
            }
        }
    }

    // DEBUG
    private IEnumerator DEBUGFlingTimer() { yield return new WaitForSeconds(2.0f); LandedEvent?.Invoke(); }

    // State management

    /// <summary>Check if the current state has a valid transition to the desired new state.</summary>
    /// <param name="newState">the desired new state.</param>
    /// <returns>true if transition to newState will work, false otherwise.</returns>
    private bool CanChangeState(State newState) {
        // result may change depending on combination of current and new state
        bool result = false;
        
        switch (currentState) {
            case State.IDLE: 
                // heart can go from idle to any other state 
                result = true;
                break;
            case State.FLUNG: 
                // heart should not get hurt while mid-fling
                result = true;
                break;
            case State.FALLING: 
                // heart should not be flung while falling
                result = (newState != State.FLUNG);
                break;
            case State.HURT: 
                // heart can go from hurt to any other state
                result = true;
                break;
            case State.DEAD: 
                // if heart is dead, cannot change state
                result = false;
                break;
            default:
                Debug.LogError("Unhandled Heart State encountered!");
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
        return true;
    }

    // Helper methods 

    /// <summary>Toggles whether the heart is able to be moved in various axes.</summary>
    /// <param name="freezeX">if true, the rigidbody's x position will not be constrained.</param>
    void ToggleFreezePosition(bool freezeX = false, bool freezeY = false, bool freezeZ = true) {
        // constraints are a bunch of bit flags, so use bitwise OR to set appropriately

        // rotation should always be constrained
        RigidbodyConstraints constraints = RigidbodyConstraints.FreezeRotation;
        
        // position is conditionally restrained
        // TODO
        
        rbody.constraints = constraints;
    }

    Vector3 CalculateDestination(float power) {
        Vector3 playerPos = player.transform.position;
        Vector3 pos = transform.position;

        // find vector to player
        var vecToPlayer = playerPos - pos;
        
        // destination is radius between player and heart, in the forward dir of the player, scaled by power.
        var vecFromPlayer = player.transform.forward * vecToPlayer.magnitude * power;
        return Vector3.zero;
    }

    /// <summary>Checks if heart is grounded using a raycast.</summary>
    /// <returns>True if the heart is grounded, otherwise false.</returns>
    private bool IsGrounded() {
        // send raycast straight downward. If it hits nothing, the player must be airborne.
        float distToGround = coll.bounds.extents.y;
        return Physics.Raycast(transform.position, -transform.up, distToGround + 0.1f);
    }
    
    // TODO actual fling consists of moving along the circle, and interpolating the radius from actual to target
    // TODO could be done using physics, and manipulating the length of the joint between heart and player, or by directly moving the heart and shrinking the radius.
}
