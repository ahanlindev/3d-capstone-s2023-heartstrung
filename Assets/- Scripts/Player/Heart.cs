using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Heart : MonoBehaviour
{
    // inspector fields
    [Tooltip("PlayerController instance for the player attached to this object")]
    [SerializeField] private PlayerController player;
    
    // private fields
    private Rigidbody rbody;

    // Emitted events
    public static event Action LandedEvent;

    // Setup/Teardown
    private void Awake() {
        if (!player) { Debug.LogError("Heart script has no Player set!"); }

        rbody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        PlayerController.flingEvent += OnPlayerFling;
    }

    private void OnDisable() {
        PlayerController.flingEvent -= OnPlayerFling;
    }

    // Event Handlers
    
    /// <summary>Subscribes to the player's fling event. Will fling this object around the player</summary>
    void OnPlayerFling(float power) {
        Debug.Log($"Flung by player with power {power}");
        //DEBUG
        StartCoroutine(DEBUGFlingTimer());
    }

    // DEBUG
    private IEnumerator DEBUGFlingTimer() { yield return new WaitForSeconds(2.0f); LandedEvent?.Invoke(); }

    // Fling helpers

    /// <summary>Toggles whether the rigidbody of the heart is able to be moved.</summary>
    /// <param name="freezePosition">if true, the rigidbody's position will be constrained, otherwise it will not.</param>
    void ToggleMobility(bool freezePosition) {
        // rotation should always be constrained
        rbody.constraints = (freezePosition) 
            ? RigidbodyConstraints.FreezeAll 
            : RigidbodyConstraints.FreezeRotation;
    }
}
