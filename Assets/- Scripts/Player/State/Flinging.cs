using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    /// <summary>State where the player is actively flinging, and waiting for the heart to land.</summary>
    public class Flinging : State
    {
        public Flinging(PlayerStateMachine stateMachine) : base("Flinging", stateMachine) { }

        public override void Enter()
        {
            _stateMachine.FlingEvent?.Invoke();
        }

        protected override void OnHurt()
        {
            _stateMachine.FlingInterruptedEvent?.Invoke();
        }

        protected override void OnPlayerFinishCharge(CallbackContext _)
        {
            base.OnPlayerFinishCharge(_);
        }
    }
}