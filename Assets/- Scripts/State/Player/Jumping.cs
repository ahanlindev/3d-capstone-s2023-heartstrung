using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace Player
{
    /// <summary>State where the player has left the ground by jumping</summary>
    public class Jumping : State
    {
        /// <summary> Used to prevent ground check from passing right after jump is pressed.</summary>
        private bool _canLand = false;

        public Jumping(PlayerStateMachine stateMachine) : base("Jumping", stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            // apply appropriate force to jump
            Vector3 jumpVec = _stateMachine.transform.up * _stateMachine.jumpPower;
            _stateMachine.rbody.AddForce(jumpVec, ForceMode.Impulse);
            _canLand = false;
            DOVirtual.DelayedCall(0.1f, () => _canLand = true, ignoreTimeScale: false);
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // return to idle if on ground
            if (IsGrounded() && _canLand)
            {
                _stateMachine.ChangeState(_stateMachine.idleState);
            }
        }

        protected override void OnPlayerAttack(CallbackContext _)
        {
            // TODO should the player be able to attack in midair?
            base.OnPlayerAttack(_);
        }

        protected override void OnPlayerStartFling(CallbackContext _)
        {
            // TODO should the player be able to fling in midair?
            base.OnPlayerStartFling(_);
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