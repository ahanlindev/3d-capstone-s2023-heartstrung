using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class Moving : State
    {
        public Moving(PlayerStateMachine stateMachine) : base("Moving", stateMachine) { }

        public override void UpdateLogic() => base.UpdateLogic();
        protected override void OnPlayerFinishFling(CallbackContext _) => base.OnPlayerFinishFling(_);
        protected override void OnHurt() => base.OnHurt();
        protected override void OnDie() => base.OnDie();
        protected override void OnHeartLanded() => base.OnHeartLanded();

        public override void Enter()
        {
            base.Enter();
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

            // cannot fling if there is no heart set
            if (_stateMachine.heart)
            {
                _stateMachine.ChangeState(_stateMachine.chargingState);
            }
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);
            // go to idle if no longer moving
            if (moveVector == Vector3.zero)
            {
                _stateMachine.ChangeState(_stateMachine.idleState);
                return;
            }

            // account for player move speed and tick rate
            moveVector *= _stateMachine.moveSpeed;
            moveVector *= Time.fixedDeltaTime;

            // find proper position and look rotation
            var newPos = _stateMachine.transform.position + moveVector;
            var newRot = Quaternion.LookRotation(moveVector.normalized, _stateMachine.transform.up);

            _stateMachine.rbody.Move(newPos, newRot);
        }
    }
}