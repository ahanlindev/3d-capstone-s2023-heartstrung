using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace Player
{
    public class HurtState : PlayerState
    {
        public HurtState(PlayerStateMachine stateMachine) : base("Hurt", stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            
            // return to idle when done with hurt animation
            DOVirtual.DelayedCall(
                delay: _stateMachine.hurtTime, 
                callback: () => _stateMachine.ChangeState(_stateMachine.idleState),
                ignoreTimeScale: false
            );
        }
    }
}