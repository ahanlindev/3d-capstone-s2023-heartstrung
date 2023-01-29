using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class DeadState : PlayerState
    {
        public DeadState(PlayerStateMachine stateMachine) : base("Dead", stateMachine) { }

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

        protected override void OnPlayerAttack(InputAction.CallbackContext _)
        {
            base.OnPlayerAttack(_);
        }

        protected override void OnPlayerJump(InputAction.CallbackContext _)
        {
            base.OnPlayerJump(_);
        }

        protected override void OnPlayerStartFling(InputAction.CallbackContext _)
        {
            base.OnPlayerStartFling(_);
        }

        protected override void OnPlayerFinishFling(InputAction.CallbackContext _)
        {
            base.OnPlayerFinishFling(_);
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);
        }
    }
}