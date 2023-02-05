using DG.Tweening;

namespace Heart {
    /// <summary>State where the heart is taking damage.</summary>
    public class Hurt : State {
        public Hurt(HeartStateMachine stateMachine) : base("Hurt", stateMachine) { }

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

        protected override bool StateIsFlingable() => true;
    }
}