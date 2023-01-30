using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class FallingState : PlayerState
    {
        public FallingState(PlayerStateMachine stateMachine) : base("Falling", stateMachine) { }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // return to idle if on ground
            if (IsGrounded()) {
                _stateMachine.ChangeState(_stateMachine.idleState);
            }
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