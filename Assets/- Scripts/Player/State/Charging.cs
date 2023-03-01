using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace Player
{
    /// <summary>State where the player is charging a fling</summary>
    public class Charging : State
    {
        public Charging(PlayerStateMachine stateMachine) : base("Charging", stateMachine) { }

        private float _power;
        private bool _goingToFling = false;

        // DOTween object that fluctuates the value of power
        private Tween _fluxTween;

        // DOTween object that faces away from heart initially
        private Tween _rotateTween;

        public override void Enter()
        {
            base.Enter();

            // should only be in this state if heart exists
            if (!_sm.heart)
            {
                _sm.ChangeState(_sm.idleState);
                return;
            }

            _goingToFling = false;
            _sm.ChargeFlingEvent?.Invoke();
            AudioManager.instance?.startFlingSoundEffect(_power);

            // fluctuate power
            StartPowerFlux();

            // tween rotate away from heart in anticipation of fling
            RotateAwayFromHeart(0.2f);

            _sm.trajectoryRenderer.ToggleRender(true);
        }

        public override void Exit()
        {
            base.Exit();

            AudioManager.instance?.finishFlingSoundEffect();

            // completes tweens if incomplete
            _sm.transform.DOComplete();
            StopPowerFlux();

            // reset state if fling was cancelled
            if (!_goingToFling)
            {
                _sm.trajectoryRenderer.ToggleRender(false);
                _sm.ChargeFlingCancelEvent?.Invoke();
            }
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            AudioManager.instance?.continueFlingSoundEffect(_power);
            if (!_rotateTween.active) { 
                RotateAwayFromHeart(0.0f); }
            UpdateFlingTrajectory();

        }

        protected override void OnPlayerAttackInput(CallbackContext _)
        {
            // allow attack to cancel a charge
            base.OnPlayerAttackInput(_);
            _sm.ChangeState(_sm.attackingState);
        }

        protected override void OnPlayerJumpInput(CallbackContext _)
        {
            // allow jump to cancel a charge
            base.OnPlayerJumpInput(_);
            _sm.ChangeState(_sm.jumpingState);
        }

        protected override void OnPlayerFinishChargeInput(CallbackContext _)
        {
            base.OnPlayerFinishChargeInput(_);

            // Fling heart if able. Can do this in Flinging.Enter, but will be harder to get power value
            if (_sm.heart.canBeFlung)
            {
                // execute fling
                _goingToFling = true;
                _sm.FlingEvent?.Invoke(_power);
                AudioManager.instance?.playSoundEvent("DodgerFling");
                _sm.ChangeState(_sm.flingingState);
            }
            else
            {
                // return to idle
                _sm.ChangeState(_sm.idleState);
            }
        }

        // Helper Methods ---------------------------------------------

        /// <summary>rotates laterally away from the heart over a short time</summary>
        private void RotateAwayFromHeart(float timeNeeded)
        {
            Vector3 vecFromHeart = _sm.transform.position - _sm.heart.transform.position;
            Vector3 lookAtPoint = _sm.transform.position + vecFromHeart;

            _rotateTween = _sm.transform.DOLookAt(lookAtPoint, timeNeeded, AxisConstraint.Y)
                .SetEase(Ease.InOutCubic);
        }

        /// <summary>Begins interpolating _power between min and max</summary>
        private void StartPowerFlux()
        {
            _power = _sm.minPower;
            float periodLength = (_sm.maxPower - _sm.minPower) / _sm.powerPerSecond;
            _fluxTween = DOVirtual.Float(_sm.minPower, _sm.maxPower, periodLength, (val) => _power = val)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
        }

        /// <summary>Stops interpolating _power</summary>
        private void StopPowerFlux()
        {
            _fluxTween?.Complete();
        }

        /// <summary>Updates the trajectory renderer so it can display predicted trajectory at current power</summary>
        private void UpdateFlingTrajectory()
        {
            Vector3 vecToHeart = _sm.heart.transform.position - _sm.transform.position;
            Vector3 vecToDest = -vecToHeart.normalized * _power * _sm.maxTetherLength;

            vecToDest.y = 0;

            _sm.trajectoryRenderer.UpdateTrajectory(
                _sm.transform.position,
                vecToHeart,
                vecToDest
            );
        }
    }
}