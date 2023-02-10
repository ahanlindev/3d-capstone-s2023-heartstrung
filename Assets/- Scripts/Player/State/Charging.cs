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
            _sm.ChargeFlingEvent?.Invoke();

            // fluctuate power
            float periodLength = (_sm.maxPower - _sm.minPower) / _sm.powerPerSecond;
            _fluxTween = DOVirtual.Float(_sm.minPower, _sm.maxPower, periodLength, (val) => _power = val)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);

            // tween rotate away from heart in anticipation of fling
            Vector3 vecFromHeart = _sm.transform.position - _sm.heart.transform.position;
            Vector3 lookAtPoint = _sm.transform.position + vecFromHeart;

            _sm.transform.DOLookAt(lookAtPoint, 0.2f, AxisConstraint.Y)
                .SetEase(Ease.InOutCubic);
        }

        public override void Exit()
        {
            base.Exit();

            // completes tweens if incomplete
            _sm.transform.DOComplete();
            _fluxTween.Complete();
        }

        int DEBUGCOUNTER = 0;
        public override void UpdateLogic()
        {
            base.UpdateLogic();

            DEBUGCOUNTER++;
            if (DEBUGCOUNTER % 4 == 0) Debug.Log($"Current Power! {_power}");
        }

        protected override void OnPlayerFinishCharge(CallbackContext _)
        {
            base.OnPlayerFinishCharge(_);

            // Fling heart if able. Can do this in Flinging.Enter, but will be harder to get power value
            if (_sm.heart.canBeFlung)
            {
                _sm.FlingEvent?.Invoke(_power);
                _sm.ChangeState(_sm.flingingState);
            }
            else
            {
                _sm.ChangeState(_sm.idleState);
            }
        }
    }
}