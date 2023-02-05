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

        protected override void OnPlayerAttack(CallbackContext _)
        {
            base.OnPlayerAttack(_);
            _sm.ChangeState(_sm.attackingState);
        }

        protected override void OnPlayerJump(CallbackContext _)
        {
            base.OnPlayerJump(_);
            _sm.ChangeState(_sm.jumpingState);
        }

        protected override void OnPlayerStartCharge(CallbackContext _)
        {
            base.OnPlayerStartCharge(_);

            // cannot fling if there is no heart set
            if (_sm.heart)
            {
                _sm.ChangeState(_sm.chargingState);
            }
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);
            if (moveVector != Vector3.zero)
            {
                _sm.ChangeState(_sm.movingState);
            }
        }

    }
}