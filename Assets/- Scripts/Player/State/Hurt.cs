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

        private Renderer rend;
        private int frameCount = 0;

        public override void Enter()
        {
            base.Enter();

            // flash on and off
            rend = _sm.GetComponentInChildren<Renderer>();
            rend.enabled = false;

            AudioManager.instance.playSoundEvent("KittyHurt");

            // return to idle when done with hurt animation
            _sm.StartCoroutine(StartTransitionTween());
        }

        public override void Exit()
        {
            base.Exit();
            rend.enabled = true;
        }
        
        public override void UpdateLogic()
        {
            base.UpdateLogic();
            frameCount++;

            if (frameCount % 4 == 0) {
                rend.enabled = !rend.enabled;
            }
        }

        /// <summary>description</summary>
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