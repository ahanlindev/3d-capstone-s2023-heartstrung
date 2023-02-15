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
            // play sound
            AudioManager.instance.playSoundEvent("KittyJump");


            // apply appropriate force to jump
            Vector3 jumpVec = _sm.transform.up * _sm.jumpPower;
            _sm.rbody.AddForce(jumpVec, ForceMode.Impulse);
            _canLand = false;
            DOVirtual.DelayedCall(0.1f, () => _canLand = true, ignoreTimeScale: false);
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // return to idle if on ground
            if (IsGrounded() && _canLand)
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