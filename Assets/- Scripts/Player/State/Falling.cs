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
                _stateMachine.ChangeState(_stateMachine.idleState);
            }
        }

        protected override void OnPlayerAttack(CallbackContext _)
        {
            // TODO should the player be able to attack in midair?
            base.OnPlayerAttack(_);
        }

        protected override void OnPlayerStartCharge(CallbackContext _)
        {
            // TODO should the player be able to fling in midair?
            base.OnPlayerStartCharge(_);
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);
            if (moveVector == Vector3.zero) { return; }
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