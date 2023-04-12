namespace Heart
{
    /// <summary>State where the heart is dead.</summary>
    public class Dead : State
    {
        public Dead(HeartStateMachine stateMachine) : base("Dead", stateMachine) { }

        protected override bool StateIsFlingable() => false;

        public override void Enter()
        {
            base.Enter();
            _sm.health.ChangeHealthEvent += AttemptRevive;
        }

        public override void Exit()
        {
            base.Exit();
            _sm.health.ChangeHealthEvent -= AttemptRevive;
        }

        protected override void OnDie()
        {
            // Do nothing. prevents dying while dead
        }

        private void AttemptRevive(float newTotal, float delta)
        {
            if (newTotal > 0) { _sm.ChangeState(_sm.idleState); }
        }
    }
}