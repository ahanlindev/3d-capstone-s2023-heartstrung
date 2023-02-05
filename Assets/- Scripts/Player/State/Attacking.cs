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
            _sm.claws.Claw(_sm.attackTime);

            // play sound
            AudioManager.instance.playSoundEvent("KittyAttack");


            // return to idle when done
            DOVirtual.DelayedCall(
                delay: _sm.attackTime,
                callback: () => _sm.ChangeState(_sm.idleState),
                ignoreTimeScale: false
            );
        }
    }
}