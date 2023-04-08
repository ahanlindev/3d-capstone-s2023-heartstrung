using DG.Tweening;

namespace Heart
{
    /// <summary>State where the heart is falling through the air.</summary>
    public class Falling : State
    {
        public Falling(HeartStateMachine stateMachine) : base("Falling", stateMachine) { }

        /// <summary> Timer used to detect if the heart is stuck in the falling state </summary>
        private Tween _fallTimer;
        
        public override void Enter()
        {
            // If falling for too long, kill and reset. Helps avoid common fling-related softlocks.
            _fallTimer = DOVirtual.DelayedCall(
                delay: _sm.fallResetTime,
                callback: () => { _sm.health.ChangeHealth(-_sm.health.maxHealth); },
                ignoreTimeScale: false
            );
        }

        public override void UpdatePhysics()
        {
            if (IsGrounded()) { _sm.ChangeState(_sm.idleState); }
        }

        public override void Exit()
        {
            base.Exit();
            if (_fallTimer.IsPlaying()) { _fallTimer.Kill(); }
            if (IsGrounded()) { _sm.LandedEvent?.Invoke(); }
        }

        protected override bool StateIsFlingable() => false;
    }
}