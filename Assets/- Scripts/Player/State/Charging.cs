using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    /// <summary>State where the player is charging a fling</summary>
    public class Charging : State
    {
        public Charging(PlayerStateMachine stateMachine) : base("Charging", stateMachine) { }

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

        protected override void OnPlayerStartCharge(CallbackContext _)
        {
            base.OnPlayerStartCharge(_);
        }

        protected override void OnPlayerFinishCharge(CallbackContext _)
        {
            base.OnPlayerFinishCharge(_);
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);
        }
    }
}