using DG.Tweening;

namespace Heart
{
    /// <summary>State where the heart is taking damage.</summary>
    public class Hurt : State
    {
        public Hurt(HeartStateMachine stateMachine) : base("Hurt", stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            // start invincibility frames
            _sm.StartInvincibility();

            AudioManager.instance.PlaySoundEvent("DodgerHurt");

            // return to idle when done with hurt animation
            DOVirtual.DelayedCall(
                delay: _sm.hurtTime,
                callback: () => _sm.ChangeState(_sm.idleState),
                ignoreTimeScale: false
            );
        }

        protected override bool StateIsFlingable() => true;
    }
}