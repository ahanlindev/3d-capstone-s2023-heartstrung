using UnityEngine;
using DG.Tweening;

namespace Heart
{
    /// <summary>State where the heart is immobile on the ground.</summary>
    public class Idle : State
    {
        public Idle(HeartStateMachine stateMachine) : base("Idle", stateMachine) { }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if (!IsGrounded())
            {
                _sm.ChangeState(_sm.fallingState);
            }
        }

        protected override void OnPlayerChargeFling()
        {
            base.OnPlayerChargeFling();

            // tween rotate away from player in anticipation of fling
            Vector3 vecFromPlayer = _sm.transform.position - _sm.player.transform.position;
            Vector3 lookAtPoint = _sm.transform.position + vecFromPlayer;

            _sm.transform.DOLookAt(lookAtPoint, 0.2f, AxisConstraint.Y)
                .SetEase(Ease.InOutCubic);
        }

        protected override void OnPlayerFling(float power)
        {
            base.OnPlayerFling(power);
            _sm.ChangeState(_sm.flungState);

            // finish rotation tween if fling happens prematurely
            _sm.transform.DOComplete();
        }

        protected override bool StateIsFlingable() => true;
    }
}