using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace Player
{
    /// <summary>State where the player is performing an attack</summary>
    public class Attacking : State
    {
        public Attacking(PlayerStateMachine stateMachine) : base("Attacking", stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            // perform attack
            _stateMachine.claws.Claw(_stateMachine.attackTime);

            // return to idle when done
            DOVirtual.DelayedCall(
                delay: _stateMachine.attackTime,
                callback: () => _stateMachine.ChangeState(_stateMachine.idleState),
                ignoreTimeScale: false
            );
        }
    }
}