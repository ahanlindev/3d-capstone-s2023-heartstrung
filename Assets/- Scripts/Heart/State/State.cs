using UnityEngine;

namespace Heart
{

    /// <summary>Ties each substate to the heart StateMachine, and handles animation upkeep</summary>
    public abstract class State : BaseState
    {
        // BaseState has _baseStateMachine, but this casts it correctly
        /// <summary>State machine for gathering information and operating on state</summary>
        protected HeartStateMachine _sm { get => (HeartStateMachine)_baseStateMachine; }

        public State(string name, HeartStateMachine stateMachine) : base(name, stateMachine) { }

        /// <summary>Event handler for when the player starts charging a fling. </summary>
        protected virtual void OnPlayerChargeFling() { }

        /// <summary>Event handler for when the player attempts to fling the heart. </summary>
        /// <param name="power">percentage power of the fling</param>
        protected virtual void OnPlayerFling(float power) { }

        /// <summary>Event handler for when the player is interrupted while flinging the heart</summary>
        protected virtual void OnPlayerFlingInterrupted() { }

        /// <summary>Event handler for when the heart gets hurt by something. Override to ignore or change base behavior.</summary>
        protected virtual void OnHurt() { 
            if (!_sm.isInvincible) { _sm.ChangeState(_sm.hurtState); } 
        }

        /// <summary>Event handler for when the heart dies. Override to ignore or change base behavior. </summary>
        protected virtual void OnDie() { _sm.ChangeState(_sm.deadState); }

        /// <summary>Event handler to allow this component to react to collisions</summary>
        protected virtual void OnCollisionEnter(Collision other) { }

        /// <summary>Check if the heart is in a flingable state. Used to update public StateMachine field. </summary>
        /// <returns>True if the heart is ready to fling. Otherwise false.</returns>
        protected abstract bool StateIsFlingable();

        /// <summary>Event handler for health changes. Decides whether to fire off OnHurt or OnDie if appropriate.</summary>
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
            _sm.player.ChargeFlingEvent += OnPlayerChargeFling;
            _sm.player.FlingEvent += OnPlayerFling;
            _sm.player.FlingInterruptedEvent += OnPlayerFlingInterrupted;
            _sm.CollisionEnterEvent += OnCollisionEnter;
            _sm.health.ChangeHealthEvent += OnChangeHealth;

            // update state machine
            _sm.canBeFlung = StateIsFlingable();

            // update animator state
            _sm.SetAnimatorBool(name, true);
        }

        public override void Exit()
        {
            base.Exit();
            _sm.player.FlingEvent -= OnPlayerFling;
            _sm.player.FlingInterruptedEvent -= OnPlayerFlingInterrupted;
            _sm.CollisionEnterEvent -= OnCollisionEnter;
            _sm.health.ChangeHealthEvent -= OnChangeHealth;

            // update animator state
            _sm.SetAnimatorBool(name, false);
        }

        // Helper Methods --------------------------------------------

        /// <summary>Check if the player is touching the ground</summary>
        /// <returns>True if player is grounded, false otherwise.</returns>
        protected bool IsGrounded()
        {
            float distToGround = _sm.coll.bounds.extents.y;
            bool touchingGround = Physics.BoxCast(
                center: _sm.transform.position,
                halfExtents: new Vector3(0.5f, 0.1f, 0.5f),
                direction: -_sm.transform.up,
                orientation: Quaternion.identity,
                maxDistance: distToGround
            );
            return touchingGround;
        }
    }
}