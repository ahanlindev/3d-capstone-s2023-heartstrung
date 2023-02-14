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
        protected virtual void OnPlayerAttackInput(CallbackContext _) { }

        /// <summary>Event handler for when the player performs a jump input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerJumpInput(CallbackContext _) { }

        /// <summary>Event handler for when the player performs a fling input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerStartChargeInput(CallbackContext _) { }

        /// <summary>Event handler for when the player finishes performing a fling input</summary>
        /// <param name="_">input context for this action. Goes unused.</param>
        protected virtual void OnPlayerFinishChargeInput(CallbackContext _) { }

        /// <summary>Event handler for when the player gets hurt by something. Override to ignore or change base behavior.</summary>
        protected virtual void OnHurt() { _sm.ChangeState(_sm.hurtState); }

        /// <summary>Event handler for when the player dies. Override to ignore or change base behavior. </summary>
        protected virtual void OnDie() { _sm.ChangeState(_sm.deadState); }

        /// <summary>Event handler for when the heart lands after being flung</summary>
        protected virtual void OnHeartLanded() { }

        /// <summary>Handler for whatever per-physics-tick movement event the player performs</summary>
        /// <param name="moveVector">Desired movement direction</param>
        protected virtual void HandlePlayerMoveInput(Vector3 moveVector) { }

        /// <summary>
        /// Event handler for health changes. Decides whether to fire off OnHurt or OnDie if appropriate. 
        /// Note that the player can only die when the heart does.
        /// </summary>
        /// <param name="newVal">new health total</param>
        /// <param name="delta">change from previous health value</param>
        private void OnChangeHealth(float newVal, float delta)
        {
            if (newVal <= 0)
            {
                // If health drops to zero, die
                OnDie();
            }
            else if (delta < 0)
            {
                // If alive, get hurt if damage was taken
                OnHurt();
            }
        }

        public override void Enter()
        {
            base.Enter();

            // subscribe to necessary events
            
            // input
            SubscribeToInputEvents();

            // pause
            PauseMenuManager.UnpauseEvent += SubscribeToInputEvents;
            PauseMenuManager.PauseEvent += UnsubscribeFromInputEvents;

            // health and heart
            _sm.hitTracker.ChangeHealthEvent += OnChangeHealth;

            if (_sm.heart)
            {
                _sm.heart.health.ChangeHealthEvent += OnChangeHealth;
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
            HandlePlayerMoveInput(moveVec3D);
        }

        public override void Exit()
        {
            base.Exit();

            // unsubscribe from events

            // input
            UnsubscribeFromInputEvents();

            // pause
            PauseMenuManager.UnpauseEvent -= SubscribeToInputEvents;
            PauseMenuManager.PauseEvent -= UnsubscribeFromInputEvents;

            // health & heart
            _sm.hitTracker.ChangeHealthEvent -= OnChangeHealth;

            if (_sm.heart)
            {
                _sm.heart.health.ChangeHealthEvent -= OnChangeHealth;
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

            Vector3 center = _sm.coll.bounds.center;
            center.y -= (distToGround - 0.4f);

            bool touchingGround = Physics.BoxCast(
                center: center,
                halfExtents: new Vector3(0.25f, 0.1f, 0.25f),
                direction: -_sm.transform.up,
                orientation: Quaternion.identity,
                maxDistance: 0.4f
            );

            return touchingGround;
        }

        private void SubscribeToInputEvents() {
            _sm.attackInput.performed += OnPlayerAttackInput;
            _sm.jumpInput.performed += OnPlayerJumpInput;
            _sm.flingInput.performed += OnPlayerStartChargeInput;
            _sm.flingInput.canceled += OnPlayerFinishChargeInput;
        }

        private void UnsubscribeFromInputEvents() {
            _sm.attackInput.performed -= OnPlayerAttackInput;
            _sm.jumpInput.performed -= OnPlayerJumpInput;
            _sm.flingInput.performed -= OnPlayerStartChargeInput;
            _sm.flingInput.canceled -= OnPlayerFinishChargeInput;
        }
    }
}