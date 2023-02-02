using UnityEngine;

namespace Heart {
    /// <summary>State where the heart is immobile on the ground.</summary>
    public class Falling : State {
        public Falling(HeartStateMachine stateMachine) : base("Falling", stateMachine) { }

        
    }
}