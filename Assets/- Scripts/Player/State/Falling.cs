using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    /// <summary>State where the player is airborne, but did leave the ground via jumping</summary>
    public class Falling : State
    {
        public Falling(PlayerStateMachine stateMachine) : base("Falling", stateMachine) { }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // return to idle if on ground
            if (IsGrounded())
            {
                _sm.ChangeState(_sm.idleState);
            }
        }

        protected override void OnPlayerAttackInput(CallbackContext _)
        {
            // TODO should the player be able to attack in midair?
            base.OnPlayerAttackInput(_);
        }

        protected override void OnPlayerStartChargeInput(CallbackContext _)
        {
            // TODO should the player be able to fling in midair?
            base.OnPlayerStartChargeInput(_);
        }

        protected override void HandlePlayerMoveInput(Vector3 moveVector)
        {
            base.HandlePlayerMoveInput(moveVector);
            if (moveVector == Vector3.zero) { return; }
            // account for player move speed and tick rate
            moveVector *= _sm.moveSpeed;
            moveVector *= Time.fixedDeltaTime;

            // find proper position and look rotation
            var newPos = _sm.transform.position + moveVector;
            var newRot = Quaternion.LookRotation(moveVector.normalized, _sm.transform.up);

            _sm.rbody.Move(newPos, newRot);
        }
    }
}