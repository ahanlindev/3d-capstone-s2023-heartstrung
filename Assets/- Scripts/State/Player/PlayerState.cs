using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerState : BaseState
    {
        // BaseState has _stateMachine, but this casts it to PlayerStateMachine
        private PlayerStateMachine _stateMachine { get => (PlayerStateMachine) _baseStateMachine; }

        public PlayerState(string name, PlayerStateMachine stateMachine) : base(name, stateMachine) { }

        /// <summary>Event handler for when the player performs a claw input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerClaw(InputAction.CallbackContext _) {}

        /// <summary>Event handler for when the player performs a jump input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerJump(InputAction.CallbackContext _) {}

        /// <summary>Event handler for when the player performs a fling input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerStartFling(InputAction.CallbackContext _) {}

        /// <summary>Event handler for when the player finishes performing a fling input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerFinishFling(InputAction.CallbackContext _) {}

        /// <summary>Handler for whatever movement event the player performs</summary>
        /// <param name="moveVector">Desired movement direction</param>
        protected virtual void HandlePlayerMove(Vector3 moveVector) {}

        public override void Enter()
        {
            base.Enter();

            // subscribe to input events
            _stateMachine._clawInput.performed += OnPlayerClaw;
            _stateMachine._jumpInput.performed += OnPlayerJump;
            _stateMachine._clawInput.performed += OnPlayerStartFling;
            _stateMachine._clawInput.performed += OnPlayerFinishFling;
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // read in player movement input and send it to handler
            Vector2 moveVec = _stateMachine._movementInput.ReadValue<Vector2>();
            Vector3 moveVec3D = new Vector3(moveVec.x, 0.0f, moveVec.y);
            HandlePlayerMove(moveVec3D);
        }

        public override void Exit()
        {
            base.Exit();

            // unsubscribe to input events
            _stateMachine._clawInput.performed -= OnPlayerClaw;
            _stateMachine._jumpInput.performed -= OnPlayerJump;
            _stateMachine._clawInput.performed -= OnPlayerStartFling;
            _stateMachine._clawInput.performed -= OnPlayerFinishFling;
        }
    }
}