using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    /// <summary>State where the player is performing an attack</summary>
    public class Attacking : State
    {
        public Attacking(PlayerStateMachine stateMachine) : base("Attacking", stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            // play sound
            AudioManager.instance?.playSoundEvent("KittyAttack");

            // Perform and finish attack
            _sm.StartCoroutine(StartTransitionTween());
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

            // perform attack
            _sm.claws.Claw(duration);

            // return to idle when done
            DOVirtual.DelayedCall(
                delay: duration,
                callback: () => _sm.ChangeState(_sm.idleState),
                ignoreTimeScale: false
            );
        }
    }
}