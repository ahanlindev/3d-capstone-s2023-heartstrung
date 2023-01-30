using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace Player
{
    public class AttackingState : PlayerState
    {
        public AttackingState(PlayerStateMachine stateMachine) : base("Attacking", stateMachine) { }

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

        public override void UpdateLogic() => base.UpdateLogic();

        public override void UpdatePhysics() => base.UpdatePhysics();

        public override void Exit() => base.Exit();
        

        protected override void OnPlayerAttack(CallbackContext _)
        {
            base.OnPlayerAttack(_);
        }

        protected override void OnPlayerJump(CallbackContext _)
        {
            base.OnPlayerJump(_);
        }

        protected override void OnPlayerStartFling(CallbackContext _)
        {
            base.OnPlayerStartFling(_);
        }

        protected override void OnPlayerFinishFling(CallbackContext _)
        {
            base.OnPlayerFinishFling(_);
        }

        protected override void HandlePlayerMove(Vector3 moveVector)
        {
            base.HandlePlayerMove(moveVector);
        }
    }
}