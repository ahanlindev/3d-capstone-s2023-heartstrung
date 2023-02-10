using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace Player
{
    /// <summary>State where the player is charging a fling</summary>
    public class Charging : State
    {
        public Charging(PlayerStateMachine stateMachine) : base("Charging", stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _sm.ChargeFlingEvent?.Invoke();

            // tween rotate away from heart in anticipation of fling
            Vector3 vecFromHeart = _sm.transform.position - _sm.heart.transform.position;
            Vector3 lookAtPoint = _sm.transform.position + vecFromHeart;

            _sm.transform.DOLookAt(lookAtPoint, 0.2f, AxisConstraint.Y)
                .SetEase(Ease.InOutCubic);
        }

        public override void Exit()
        {
            base.Exit();

            // completes rotation tween if incomplete
            _sm.transform.DOComplete(); 
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            // TODO incorporate charging/targeting
        }

        protected override void OnPlayerFinishCharge(CallbackContext _)
        {
            base.OnPlayerFinishCharge(_);
            _sm.ChangeState(_sm.flingingState);
        }
    }
}