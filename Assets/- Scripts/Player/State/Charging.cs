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

        // DOTween object that fluctuates the value of power
        private Tween _fluxTween;

        public override void Enter()
        {
            base.Enter();

            // should only be in this state if heart exists
            if (!_sm.heart) { 
                _sm.ChangeState(_sm.idleState); 
                return;
            }

            _sm.ChargeFlingEvent?.Invoke();

            // fluctuate power
            StartPowerFlux();

            // tween rotate away from heart in anticipation of fling
            RotateAwayFromHeart();

            _sm.trajectoryRenderer.ToggleRender(true);
        }

        public override void Exit()
        {
            base.Exit();

            // completes tweens if incomplete
            _sm.transform.DOComplete();
            StopPowerFlux();
        }

        int DEBUGCOUNTER = 0;
        public override void UpdateLogic()
        {
            base.UpdateLogic();

            UpdateFlingTrajectory();
    
            DEBUGCOUNTER++;
            if (DEBUGCOUNTER % 4 == 0) Debug.Log($"Current Power! {_power}");
        }

        protected override void OnPlayerFinishCharge(CallbackContext _)
        {
            base.OnPlayerFinishCharge(_);

            // Fling heart if able. Can do this in Flinging.Enter, but will be harder to get power value
            if (_sm.heart.canBeFlung)
            {
                // execute fling
                _sm.FlingEvent?.Invoke(_power);
                _sm.ChangeState(_sm.flingingState);
            }
            else
            {
                // cancel trajectory rendering and return to idle
                _sm.trajectoryRenderer.ToggleRender(false);
                _sm.ChangeState(_sm.idleState);
            }
        }

        // Helper Methods ---------------------------------------------
        
        /// <summary>rotates laterally away from the heart over a short time</summary>
        private void RotateAwayFromHeart() {
            Vector3 vecFromHeart = _sm.transform.position - _sm.heart.transform.position;
            Vector3 lookAtPoint = _sm.transform.position + vecFromHeart;

            _sm.transform.DOLookAt(lookAtPoint, 0.2f, AxisConstraint.Y)
                .SetEase(Ease.InOutCubic);
        }
        
        /// <summary>Begins interpolating _power between min and max</summary>
        private void StartPowerFlux() {
            _power = _sm.minPower;
            float periodLength = (_sm.maxPower - _sm.minPower) / _sm.powerPerSecond;
            _fluxTween = DOVirtual.Float(_sm.minPower, _sm.maxPower, periodLength, (val) => _power = val)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
        }

        /// <summary>Stops interpolating _power</summary>
        private void StopPowerFlux() {
            _fluxTween?.Complete();
        }

        /// <summary>Updates the trajectory renderer so it can display predicted trajectory at current power</summary>
        private void UpdateFlingTrajectory() {
            Vector3 vecToHeart = _sm.heart.transform.position - _sm.transform.position;
            Vector3 vecToDest = -vecToHeart.normalized * _power * _sm.maxTetherLength;
            Debug.Log($"Power is {_power}");
            vecToDest.y = 0;

            _sm.trajectoryRenderer.UpdateTrajectory(
                _sm.transform.position, 
                vecToHeart,
                vecToDest
            );
        }
    }
}