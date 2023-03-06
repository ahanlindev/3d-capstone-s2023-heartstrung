using UnityEngine;
using DG.Tweening;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    /// <summary>State where the player is on the ground and actively in motion</summary>
    public class Moving : State
    {
        public Moving(PlayerStateMachine stateMachine) : base("Moving", stateMachine) { }

        /// <summary>Timer used to count down coyote time after becoming airborne</summary>
        private Tween _coyoteTimer;

        public override void Enter()
        {
            base.Enter();
            _coyoteTimer = DOVirtual.DelayedCall(
                delay: _sm.coyoteTime,
                callback: () => _sm.ChangeState(_sm.fallingState),
                ignoreTimeScale: false
            );

            // we only want to start the timer when leaving the ground
            _coyoteTimer.Pause();
        }

        public override void Exit()
        {
            // kill timer if necessary
            if (_coyoteTimer?.active ?? false) {
                _coyoteTimer.Kill();
            }
            base.Exit();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if (!IsGrounded())
            {
                // left the ground. Start falling after grace period
                _coyoteTimer?.Play();
            } else {
                // reset grace period and pause timer.
                _coyoteTimer?.Rewind();
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
            // go to idle if no longer moving
            if (moveVector == Vector3.zero)
            {
                _sm.ChangeState(_sm.idleState);
                return;
            }

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