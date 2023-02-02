using UnityEngine;

namespace Heart {
    /// <summary>State where the heart is immobile on the ground.</summary>
    public class Dead : State {
        public Dead(HeartStateMachine stateMachine) : base("Dead", stateMachine) { }

        
    }
}