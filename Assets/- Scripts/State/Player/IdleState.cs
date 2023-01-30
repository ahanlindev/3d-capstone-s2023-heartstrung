using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerStateMachine stateMachine) : base("Idle", stateMachine) { }

        public override void UpdateLogic() => base.UpdateLogic();
        protected override void OnPlayerFinishFling(CallbackContext _) => base.OnPlayerFinishFling(_);
        protected override void OnHurt() => base.OnHurt();
        protected override void OnDie() => base.OnDie();
        protected override void OnHeartLanded() => base.OnHeartLanded();

        public override void Enter()
        {
            base.Enter();
        }

        public override void UpdatePhysics() { 
            base.UpdatePhysics();
            if (!IsGrounded()) {
                _stateMachine.ChangeState(_stateMachine.fallingState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        protected override void OnPlayerAttack(CallbackContext _)
        {
            base.OnPlayerAttack(_);
            _stateMachine.ChangeState(_stateMachine.attackingState);
        }

        protected override void OnPlayerJump(CallbackContext _)
        {
            base.OnPlayerJump(_);
            _stateMachine.ChangeState(_stateMachine.jumpingState);
        }

        protected override void OnPlayerStartFling(CallbackContext _)
        {
            base.OnPlayerStartFling(_);
            _stateMachine.ChangeState(_stateMachine.chargingState);
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);
            if (moveVector != Vector3.zero)
            {
                _stateMachine.ChangeState(_stateMachine.movingState);
            }
        }

    }
}