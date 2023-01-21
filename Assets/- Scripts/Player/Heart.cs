using System;
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
    }

    // State manipulators
    void ToggleMobility(bool willFreeze) {

    }
}
