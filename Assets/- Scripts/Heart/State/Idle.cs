using UnityEngine;
using DG.Tweening;

namespace Heart
{
    /// <summary>State where the heart is immobile on the ground.</summary>
    public class Idle : State
    {
        public Idle(HeartStateMachine stateMachine) : base("Idle", stateMachine) { }

        // true if player is charging a fling. TODO could be its own state
        private bool _playerCharging;

        // tween that rotates away from the player
        private Tween _rotateTween;

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if (!IsGrounded())
            {
                _sm.ChangeState(_sm.fallingState);
            }

            // constantly face away from player after initial tween
            if (!_playerCharging) { return; }
            if (_rotateTween is not { active: true }) {
                RotateAwayFromPlayer(0.0f);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _rotateTween?.Complete();
        }

        protected override void OnPlayerChargeFling()
        {
            base.OnPlayerChargeFling();

            _playerCharging = true;
            RotateAwayFromPlayer(0.2f);
        }

        protected override void OnPlayerChargeFlingCancel()
        {
            base.OnPlayerChargeFlingCancel();
            _playerCharging = false;
        }

        protected override void OnPlayerFling(float power)
        {
            base.OnPlayerFling(power);
            _playerCharging = false;

            _sm.ChangeState(_sm.flungState);

            // finish rotation tween if fling happens prematurely
            _sm.transform.DOComplete();
        }

        protected override bool StateIsFlingable() => true;
        

        private void RotateAwayFromPlayer(float timeNeeded) {
            // tween rotate away from player in anticipation of fling
            Vector3 vecFromPlayer = _sm.transform.position - _sm.player.transform.position;
            Vector3 lookAtPoint = _sm.transform.position + vecFromPlayer;

            _rotateTween = _sm.transform.DOLookAt(lookAtPoint, timeNeeded, AxisConstraint.Y)
                .SetEase(Ease.InOutCubic);
        }
    }
}