namespace Player
{
    public class MovingState : PlayerState
    {
        public MovingState(PlayerStateMachine stateMachine) : base("Moving", stateMachine) { }
        
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