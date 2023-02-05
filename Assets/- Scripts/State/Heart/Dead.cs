using UnityEngine;

namespace Heart
{
    /// <summary>State where the heart is dead.</summary>
    public class Dead : State
    {
        public Dead(HeartStateMachine stateMachine) : base("Dead", stateMachine) { }

        protected override bool StateIsFlingable() => false;
    }
}