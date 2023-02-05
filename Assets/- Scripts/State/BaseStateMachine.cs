using UnityEngine;

/// <summary>Component that runs logic drawn from its current state</summary>
public abstract class BaseStateMachine : MonoBehaviour
{
    private BaseState _currentState;

    // Initialize the state machine.
    private void Start()
    {
        _currentState = GetInitialState();
        _currentState?.Enter();
    }

    // perform whatever per-frame logic the current state demands
    private void Update() => _currentState?.UpdateLogic();

    // perform whatever per-physics-tick logic the current state demands
    private void FixedUpdate() => _currentState?.UpdatePhysics();

    /// <summary>Returns the desired initial state for the state machine.</summary>
    protected abstract BaseState GetInitialState();

    public void ChangeState(BaseState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    // DEBUG
    protected virtual void OnGUI() {
        string content = _currentState != null ? _currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    }
}
