namespace Player
{
    public class HurtState : BaseState
    {
        // BaseState has _stateMachine, but this casts it to PlayerStateMachine
        private PlayerStateMachine _sm { get => (PlayerStateMachine) _stateMachine; }

        public HurtState(PlayerStateMachine stateMachine) : base("Hurt", stateMachine) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}