using UnityEngine;

namespace Heart
{
    /// <summary>State where the heart is falling through the air.</summary>
    public class Falling : State
    {
        public Falling(HeartStateMachine stateMachine) : base("Falling", stateMachine) { }

        public override void UpdatePhysics()
        {
            if (IsGrounded()) { _sm.ChangeState(_sm.idleState); }
        }

        public override void Exit()
        {
            base.Exit();
            if (IsGrounded()) { _sm.LandedEvent?.Invoke(); }
        }

        protected override bool StateIsFlingable() => false;
    }
}