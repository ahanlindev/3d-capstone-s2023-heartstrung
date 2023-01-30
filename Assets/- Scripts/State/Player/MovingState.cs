using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class MovingState : PlayerState
    {
        public MovingState(PlayerStateMachine stateMachine) : base("Moving", stateMachine) { }

        public override void UpdateLogic() => base.UpdateLogic();
        public override void UpdatePhysics() => base.UpdatePhysics();
        protected override void OnPlayerStartFling(CallbackContext _) => base.OnPlayerStartFling(_);
        protected override void OnPlayerFinishFling(CallbackContext _) => base.OnPlayerFinishFling(_);
        protected override void OnHurt() => base.OnHurt();
        protected override void OnDie() => base.OnDie();
        protected override void OnHeartLanded() => base.OnHeartLanded();
        
        public override void Enter()
        {
            base.Enter();
            _stateMachine.anim.SetBool("Moving", true);
        }

        public override void Exit()
        {
            base.Exit();
            _stateMachine.anim.SetBool("Moving", false);
        }

        protected override void OnPlayerAttack(CallbackContext _)
        {
            base.OnPlayerAttack(_);
            _stateMachine.ChangeState(_stateMachine.attackingState);
        }

        protected override void OnPlayerJump(CallbackContext _)
        {
            // TODO implement jump
            base.OnPlayerJump(_);
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);

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