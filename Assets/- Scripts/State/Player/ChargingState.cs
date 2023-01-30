using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class ChargingState : PlayerState
    {
        public ChargingState(PlayerStateMachine stateMachine) : base("Charging", stateMachine) { }

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

        protected override void OnPlayerAttack(CallbackContext _)
        {
            base.OnPlayerAttack(_);
        }

        protected override void OnPlayerJump(CallbackContext _)
        {
            base.OnPlayerJump(_);
        }

        protected override void OnPlayerStartFling(CallbackContext _)
        {
            base.OnPlayerStartFling(_);
        }

        protected override void OnPlayerFinishFling(CallbackContext _)
        {
            base.OnPlayerFinishFling(_);
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);
        }
    }
}