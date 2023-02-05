using UnityEngine;

namespace Heart
{
    /// <summary>State where the heart is immobile on the ground.</summary>
    public class Idle : State
    {
        public Idle(HeartStateMachine stateMachine) : base("Idle", stateMachine) { }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if (!IsGrounded())
            {
                _sm.ChangeState(_sm.fallingState);
            }
        }

        protected override void OnPlayerFling(float power)
        {
            base.OnPlayerFling(power);
            _sm.ChangeState(_sm.flungState);
        }

        protected override bool StateIsFlingable() => true;
    }
}