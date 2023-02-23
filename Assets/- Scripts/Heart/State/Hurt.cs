using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;

namespace Heart
{
    /// <summary>State where the heart is taking damage.</summary>
    public class Hurt : State
    {
        public Hurt(HeartStateMachine stateMachine) : base("Hurt", stateMachine) { }

        // For some reason, turning the renderers off doesn't work correctly. TODO this is fragile
        private GameObject rig;
        private bool[] _rendererEntryState;
        private bool _currentlyOn;
        private float _frameCount = 0;

        public override void Enter()
        {
            base.Enter();

            // flash on and off
            rig = _sm.transform.GetChild(0).gameObject;

            AudioManager.instance.playSoundEvent("DodgerHurt");

            // return to idle when done with hurt animation
            DOVirtual.DelayedCall(
                delay: _sm.hurtTime,
                callback: () => _sm.ChangeState(_sm.idleState),
                ignoreTimeScale: false
            );
        }

        public override void Exit()
        {
            base.Exit();
            Flicker(true);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            _frameCount++;

            // flicker on every 4th frame
            if (_frameCount % 4 == 0)
            {
                _currentlyOn = !_currentlyOn;
                Flicker(_currentlyOn);
            }
        }

        protected override bool StateIsFlingable() => true;

        /// <summary>Toggles rig renderer on and off as appropriate</summary>
        /// <param name="on">If true, model will be turned on. Otherwise it will be turned off.</param>
        private void Flicker(bool on)
        {
            rig.SetActive(on);
        }
    }
}