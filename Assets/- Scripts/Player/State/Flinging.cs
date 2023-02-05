using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    /// <summary>State where the player is actively flinging, and waiting for the heart to land.</summary>
    public class Flinging : State
    {
        public Flinging(PlayerStateMachine stateMachine) : base("Flinging", stateMachine) { }

        private bool _heartHasLanded = false;

        public override void Enter()
        {
            base.Enter();
            _heartHasLanded = false;

            Transform tf = _stateMachine.transform;
            Transform heartTf = _stateMachine.heart.transform;

            // face player directly away from the heart
            Vector3 vecToHeart = heartTf.position - tf.position;
            vecToHeart.y = 0.0f; // don't want to rotate player too much
            _stateMachine.rbody.MoveRotation(Quaternion.LookRotation(-vecToHeart, tf.up));

            _stateMachine.FlingEvent?.Invoke(1.0f);
        }

        public override void Exit()
        {
            base.Exit();
            if (!_heartHasLanded)
            {
                _stateMachine.FlingInterruptedEvent?.Invoke();
            }
        }

        protected override void OnHeartLanded()
        {
            base.OnHeartLanded();
            _heartHasLanded = true;
            _stateMachine.ChangeState(_stateMachine.idleState);
        }
    }
}