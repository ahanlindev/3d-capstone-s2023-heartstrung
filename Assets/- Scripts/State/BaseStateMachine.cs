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

    // exit state when destroyed to make sure event subscriptions are undone
    private void OnDestroy() {
        _currentState.Exit();
        _currentState = null;
    }

    /// <summary>
    /// Initialization method used to set state on startup
    /// </summary>
    /// <returns>The state that this state machine should start in</returns>
    protected abstract BaseState GetInitialState();
    
    /// <summary>
    /// Exit the current state and enter a new state
    /// </summary>
    /// <param name="newState">The desired state to transition into</param>
    public void ChangeState(BaseState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
