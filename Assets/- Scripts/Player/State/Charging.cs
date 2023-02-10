using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    /// <summary>State where the player is charging a fling</summary>
    public class Charging : State
    {
        public Charging(PlayerStateMachine stateMachine) : base("Charging", stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _sm.ChargeFlingEvent?.Invoke();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            // TODO incorporate charging/targeting
        }

        protected override void OnPlayerFinishCharge(CallbackContext _)
        {
            base.OnPlayerFinishCharge(_);
            _sm.ChangeState(_sm.flingingState);
        }
    }
}