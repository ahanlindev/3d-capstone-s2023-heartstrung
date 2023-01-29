using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerState : BaseState
    {
        // BaseState has _stateMachine, but this casts it to PlayerStateMachine
        protected PlayerStateMachine _stateMachine { get => (PlayerStateMachine)_baseStateMachine; }

        public PlayerState(string name, PlayerStateMachine stateMachine) : base(name, stateMachine) { }

        /// <summary>Event handler for when the player performs an attack input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerAttack(InputAction.CallbackContext _) { }

        /// <summary>Event handler for when the player performs a jump input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerJump(InputAction.CallbackContext _) { }

        /// <summary>Event handler for when the player performs a fling input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerStartFling(InputAction.CallbackContext _) { }

        /// <summary>Event handler for when the player finishes performing a fling input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerFinishFling(InputAction.CallbackContext _) { }

        /// <summary>Event handler for when the heart lands after being flung</summary>
        protected virtual void OnHeartLanded() {}

        /// <summary>Handler for whatever movement event the player performs</summary>
        /// <param name="moveVector">Desired movement direction</param>
        protected virtual void HandlePlayerMove(Vector3 moveVector) { }

        public override void Enter()
        {
            base.Enter();

            // subscribe to events
            _stateMachine.attackInput.performed += OnPlayerAttack;
            _stateMachine.jumpInput.performed += OnPlayerJump;
            _stateMachine.attackInput.performed += OnPlayerStartFling;
            _stateMachine.attackInput.performed += OnPlayerFinishFling;
            _stateMachine.heart.LandedEvent += OnHeartLanded;
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // read in player movement input and send it to handler
            Vector2 moveVec = _stateMachine.movementInput.ReadValue<Vector2>();
            Vector3 moveVec3D = new Vector3(moveVec.x, 0.0f, moveVec.y);
            HandlePlayerMove(moveVec3D);
        }

        public override void Exit()
        {
            base.Exit();

            // unsubscribe to input events
            _stateMachine.attackInput.performed -= OnPlayerAttack;
            _stateMachine.jumpInput.performed -= OnPlayerJump;
            _stateMachine.attackInput.performed -= OnPlayerStartFling;
            _stateMachine.attackInput.performed -= OnPlayerFinishFling;
            _stateMachine.heart.LandedEvent -= OnHeartLanded;
        }
    }
}