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
                _sm.ChangeState(_sm.fallingState);
            }
        }

        protected override void OnPlayerAttackInput(CallbackContext _)
        {
            base.OnPlayerAttackInput(_);
            _sm.ChangeState(_sm.attackingState);
        }

        protected override void OnPlayerJumpInput(CallbackContext _)
        {
            base.OnPlayerJumpInput(_);
            _sm.ChangeState(_sm.jumpingState);
        }

        protected override void OnPlayerStartChargeInput(CallbackContext _)
        {
            base.OnPlayerStartChargeInput(_);

            // cannot fling if there is no heart set
            if (_sm.heart)
            {
                _sm.ChangeState(_sm.chargingState);
            }
        }

        protected override void HandlePlayerMoveInput(Vector3 moveVector)
        {
            base.HandlePlayerMoveInput(moveVector);

            // enter moving state if able to move
            if (moveVector != Vector3.zero)
            {
                if (_sm.moveSpeed > 0) {
                    _sm.ChangeState(_sm.movingState);
                }
                else {
                    // allow player to rotate-in-place while immobilized
                    var newRot = Quaternion.LookRotation(moveVector.normalized, _sm.transform.up);
                    _sm.rbody.MoveRotation(newRot);
                }
            }

        }

    }
}