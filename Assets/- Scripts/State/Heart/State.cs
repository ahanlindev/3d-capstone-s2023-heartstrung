using UnityEngine;

namespace Heart
{

    /// <summary>Ties each substate to the heart StateMachine, and handles animation upkeep</summary>
    public abstract class State : BaseState
    {
        // BaseState has _baseStateMachine, but this casts it correctly
        /// <summary>State machine for gathering information and operating on state</summary>
        protected HeartStateMachine _stateMachine { get => (HeartStateMachine)_baseStateMachine; }

        public State(string name, HeartStateMachine stateMachine) : base(name, stateMachine) { }

        /// <summary>Event handler for when the player attempts to fling the heart. </summary>
        protected virtual void OnPlayerFling(float power) { }

        /// <summary>Event handler for when the player is interrupted while flinging the heart</summary>
        protected virtual void OnPlayerFlingInterrupted() { }

        /// <summary>Event handler for when the heart gets hurt by something. Override to ignore or change base behavior.</summary>
        protected virtual void OnHurt() { _stateMachine.ChangeState(_stateMachine.hurtState); }

        /// <summary>Event handler for when the heart dies. Override to ignore or change base behavior. </summary>
        protected virtual void OnDie() { _stateMachine.ChangeState(_stateMachine.deadState); }

        /// <summary>Event handler to allow this component to react to collisions</summary>
        protected virtual void OnCollisionEnter(Collision other) { }

        /// <summary>Check if the heart is in a flingable state. Used to update public StateMachine field. </summary>
        /// <returns>True if the heart is ready to fling. Otherwise false.</returns>
        protected abstract bool StateIsFlingable();

        public override void Enter()
        {
            base.Enter();
            _stateMachine.player.FlingEvent += OnPlayerFling;
            _stateMachine.player.FlingInterruptedEvent += OnPlayerFlingInterrupted;
            _stateMachine.CollisionEnterEvent += OnCollisionEnter;

            // Todo set up hurt and die

            // update state machine
            _stateMachine.canBeFlung = StateIsFlingable();

            // update animator state
            _stateMachine.SetAnimatorBool(name, true);
        }

        public override void Exit()
        {
            base.Exit();
            _stateMachine.player.FlingEvent -= OnPlayerFling;
            _stateMachine.player.FlingInterruptedEvent -= OnPlayerFlingInterrupted;
            _stateMachine.CollisionEnterEvent -= OnCollisionEnter;
            // Todo tear down hurt and die

            // update animator state
            _stateMachine.SetAnimatorBool(name, false);
        }

        // Helper Methods --------------------------------------------

        /// <summary>Check if the player is touching the ground</summary>
        /// <returns>True if player is grounded, false otherwise.</returns>
        protected bool IsGrounded()
        {
            float distToGround = _stateMachine.coll.bounds.extents.y;
            bool touchingGround = Physics.BoxCast(
                center: _stateMachine.transform.position,
                halfExtents: new Vector3(0.5f, 0.1f, 0.5f),
                direction: -_stateMachine.transform.up,
                orientation: Quaternion.identity,
                maxDistance: distToGround
            );
            return touchingGround;
        }
    }
}