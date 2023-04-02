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

            Transform tf = _sm.transform;
            Transform heartTf = _sm.heart.transform;

            // face player directly away from the heart
            Vector3 vecToHeart = heartTf.position - tf.position;
            vecToHeart.y = 0.0f; // don't want to rotate player too much
            _sm.rbody.MoveRotation(Quaternion.LookRotation(-vecToHeart, tf.up));
        }

        public override void Exit()
        {
            base.Exit();
            _sm.trajectoryRenderer.ToggleRender(false);
            
            // rubber-band heart and player (hopefully) without launching them
            if (!_sm.heart) { return; }
            Vector3 heartToPlayer = _sm.transform.position - _sm.heart.transform.position;
            float distToHeart = heartToPlayer.magnitude;
            heartToPlayer = heartToPlayer.normalized * Mathf.Clamp(distToHeart, 0, _sm.maxTetherLength);
            _sm.rbody.MovePosition(_sm.heart.transform.position + heartToPlayer);

            _sm.rbody.velocity = Vector3.zero;
            _sm.heart.rbody.velocity = Vector3.zero;

            if (!_heartHasLanded)
            {
                _sm.FlingInterruptedEvent?.Invoke();
            }
        }

        protected override void OnHeartLanded()
        {
            base.OnHeartLanded();
            _heartHasLanded = true;
            _sm.ChangeState(_sm.idleState);
            AudioManager.instance?.playSoundEvent("DodgerLand");
        }
    }
}