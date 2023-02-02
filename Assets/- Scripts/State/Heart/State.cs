using UnityEngine;

namespace Heart {

    /// <summary>Ties each substate to the heart StateMachine, and handles animation upkeep</summary>
    public abstract class State : BaseState
    {
        // BaseState has _baseStateMachine, but this casts it correctly
        /// <summary>State machine for gathering information and operating on state</summary>
        protected HeartStateMachine _stateMachine {get => (HeartStateMachine) _baseStateMachine; }

        public State(string name, HeartStateMachine stateMachine) : base(name, stateMachine) { }

        protected virtual void OnPlayerFling() { }

        /// <summary>Event handler for when the heart gets hurt by something. Override to ignore or change base behavior.</summary>
        protected virtual void OnHurt() { _stateMachine.ChangeState(_stateMachine.hurtState); }

        /// <summary>Event handler for when the heart dies. Override to ignore or change base behavior. </summary>
        protected virtual void OnDie() { _stateMachine.ChangeState(_stateMachine.deadState); }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.player.FlingEvent += OnPlayerFling;
            // Todo set up hurt and die
        }

        public override void Exit()
        {
            base.Exit();
            _stateMachine.player.FlingEvent -= OnPlayerFling;
            // Todo tear down hurt and die
        }
    }

}