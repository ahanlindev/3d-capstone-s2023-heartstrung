using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace Player
{
    /// <summary>State where the player is dead and cannot perform other actions</summary>
    public class Dead : State
    {
        public Dead(PlayerStateMachine stateMachine) : base("Dead", stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            // TODO this is a hard-coded value. Likely better to handle this in some scene manager class
            DOVirtual.DelayedCall(
                1f,
                () =>
                {
                    int index = SceneManager.GetActiveScene().buildIndex;
                    SceneManager.LoadSceneAsync(index);
                }, 
                false
            );
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        public override void Exit()
        {
            base.Exit();
        }

        protected override void OnPlayerAttackInput(CallbackContext _)
        {
            base.OnPlayerAttackInput(_);
        }

        protected override void OnPlayerJumpInput(CallbackContext _)
        {
            base.OnPlayerJumpInput(_);
        }

        protected override void OnPlayerStartChargeInput(CallbackContext _)
        {
            base.OnPlayerStartChargeInput(_);
        }

        protected override void OnPlayerFinishChargeInput(CallbackContext _)
        {
            base.OnPlayerFinishChargeInput(_);
        }

        protected override void HandlePlayerMoveInput(Vector3 moveVector)
        {
            base.HandlePlayerMoveInput(moveVector);
        }
    }
}