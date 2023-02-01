using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    /// <summary>State where the player is on the ground and not performing any other actions</summary>
    public class Idle : State
    {
        public Idle(PlayerStateMachine stateMachine) : base("Idle", stateMachine) { }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if (!IsGrounded())
            {
                _stateMachine.ChangeState(_stateMachine.fallingState);
            }
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
            if (moveVector != Vector3.zero)
            {
                _stateMachine.ChangeState(_stateMachine.movingState);
            }
        }

    }
}