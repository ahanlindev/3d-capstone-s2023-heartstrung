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

            // return to idle when done with hurt animation
            DOVirtual.DelayedCall(
                delay: _sm.hurtTime,
                callback: () => _sm.ChangeState(_sm.idleState),
                ignoreTimeScale: false
            );
        }
    }
}