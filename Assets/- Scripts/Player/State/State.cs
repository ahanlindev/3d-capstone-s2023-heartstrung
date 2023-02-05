using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    /// <summary>Ties each substate to the player StateMachine, and handles input/animation upkeep</summary>
    public abstract class State : BaseState
    {
        // BaseState has _baseStateMachine, but this casts it to PlayerStateMachine
        /// <summary>State machine for gathering information and operating on state</summary>
        protected PlayerStateMachine _sm { get => (PlayerStateMachine)_baseStateMachine; }

        public State(string name, PlayerStateMachine stateMachine) : base(name, stateMachine) { }

        /// <summary>Event handler for when the player performs an attack input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerAttack(CallbackContext _) { }

        /// <summary>Event handler for when the player performs a jump input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerJump(CallbackContext _) { }

        /// <summary>Event handler for when the player performs a fling input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerStartCharge(CallbackContext _) { }

        /// <summary>Event handler for when the player finishes performing a fling input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerFinishCharge(CallbackContext _) { }

        /// <summary>Event handler for when the player gets hurt by something. Override to ignore or change base behavior.</summary>
        protected virtual void OnHurt() { _sm.ChangeState(_sm.hurtState); }

        /// <summary>Event handler for when the player dies. Override to ignore or change base behavior. </summary>
        protected virtual void OnDie() { _sm.ChangeState(_sm.deadState); }

        /// <summary>Event handler for when the heart lands after being flung</summary>
        protected virtual void OnHeartLanded() { }

        /// <summary>Handler for whatever per-physics-tick movement event the player performs</summary>
        /// <param name="moveVector">Desired movement direction</param>
        protected virtual void HandlePlayerMove(Vector3 moveVector) { }

        public override void Enter()
        {
            base.Enter();

            // subscribe to events
            _sm.attackInput.performed += OnPlayerAttack;
            _sm.jumpInput.performed += OnPlayerJump;
            _sm.flingInput.performed += OnPlayerStartCharge;
            _sm.flingInput.canceled += OnPlayerFinishCharge;
            _sm.PlayerHurtEvent += OnHurt;

            if (_sm.heart)
            {
                _sm.heart.LandedEvent += OnHeartLanded;
            }

            // set animator state
            _sm.SetAnimatorBool(name, true);
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // read in player movement input and send it to handler
            Vector2 moveVec = _sm.movementInput.ReadValue<Vector2>();
            Vector3 moveVec3D = new Vector3(moveVec.x, 0.0f, moveVec.y);
            HandlePlayerMove(moveVec3D);
        }

        public override void Exit()
        {
            base.Exit();

            // unsubscribe to input events
            _sm.attackInput.performed -= OnPlayerAttack;
            _sm.jumpInput.performed -= OnPlayerJump;
            _sm.flingInput.performed -= OnPlayerStartCharge;
            _sm.flingInput.canceled -= OnPlayerFinishCharge;
            _sm.PlayerHurtEvent -= OnHurt;

            if (_sm.heart)
            {
                _sm.heart.LandedEvent -= OnHeartLanded;
            }

            // reset animator state
            _sm.SetAnimatorBool(name, false);
        }

        /// <summary>Check if the player is touching the ground</summary>
        /// <returns>True if player is grounded, false otherwise.</returns>
        protected bool IsGrounded()
        {
            float distToGround = _sm.coll.bounds.extents.y;
            Vector3 collOffset = _sm.coll.bounds.center - _sm.transform.position;
            bool touchingGround = Physics.BoxCast(
                center: _sm.transform.position + collOffset,
                halfExtents: new Vector3(0.5f, 0.1f, 0.5f),
                direction: -_sm.transform.up,
                orientation: Quaternion.identity,
                maxDistance: distToGround
            );
            return touchingGround;
        }
    }
}