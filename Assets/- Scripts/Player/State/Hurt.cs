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

            // return to idle when done with hurt animation
            DOVirtual.DelayedCall(
                delay: _sm.hurtTime,
                callback: () => _sm.ChangeState(_sm.idleState),
                ignoreTimeScale: false
            );
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
    }
}