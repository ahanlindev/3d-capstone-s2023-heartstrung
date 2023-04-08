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
            // TODO this is a hard-coded value. Likely better to handle this with animator clip info
            DOVirtual.DelayedCall(
                2f,
                () =>
                {
                    var cm = Object.FindObjectOfType<CheckpointManager>();
                    cm?.ResetToCheckpoint();
                    _sm.ChangeState(_sm.idleState);
                },
                false
            );
        }

        protected override void OnDie()
        {
            // empty. Prevents dying while dead
        }
        
    }
}