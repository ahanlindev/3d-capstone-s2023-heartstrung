using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace Player
{
    /// <summary>State where the player is temporarily immobilized by being hurt.</summary>
    public class Hurt : State
    {
        public Hurt(PlayerStateMachine stateMachine) : base("Hurt", stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            // flash on and off
            _sm.StartInvincibility();

            // return to idle when done with hurt animation
            _sm.StartCoroutine(StartTransitionTween());
        }
    
        /// <summary>
        /// Transition back to the idle state once the hurt 
        /// animation has concluded
        /// </summary>
        private IEnumerator StartTransitionTween() {
            float duration = 0;

            // wait a short time so animator enters attack state
            yield return null;
            yield return new WaitUntil(() => {
                duration = _sm.GetAnimatorClipLength();
                return duration > 0;
            }); 

            // return to idle when done
            DOVirtual.DelayedCall(
                delay: duration,
                callback: () => _sm.ChangeState(_sm.idleState),
                ignoreTimeScale: false
            );
        }
    }
}