using UnityEngine;

namespace Player
{
    public class StateMachine : MonoBehaviour
    {
        private BaseState _currentState;

        // Initialize the state machine.
        private void Start() {
            _currentState?.Enter();
        }

        private void Update() => _currentState?.UpdateLogic();

        private void FixedUpdate() => _currentState?.UpdatePhysics();

        protected virtual BaseState GetInitialState() { return null; }

        // Debug. TODO remove when unnecessary
        private void OnGUI() {
            string stateName = (_currentState != null) ? _currentState.name : "Null State!";
            GUILayout.Label($"<size=40><color=black>Current State:</color> <color=blue>{stateName}</color></size>");
        }
    }
}
