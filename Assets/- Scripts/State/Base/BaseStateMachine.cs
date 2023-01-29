using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    private BaseState _currentState;

    // Initialize the state machine.
    private void Start() {
        _currentState?.Enter();
    }

    // perform whatever per-frame logic the current state demands
    private void Update() => _currentState?.UpdateLogic();

    // perform whatever per-physics-tick logic the current state demands
    private void FixedUpdate() => _currentState?.UpdatePhysics();

    /// <summary>Returns the desired initial state for the state machine. In the base class, this is null.</summary>
    protected virtual BaseState GetInitialState() { return null; }

    // Debug. TODO remove when unnecessary
    private void OnGUI() {
        string stateName = (_currentState != null) ? _currentState.name : "Null State!";
        GUILayout.Label($"<size=40><color=black>Current State:</color> <color=blue>{stateName}</color></size>");
    }
}
