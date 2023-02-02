using UnityEngine;

namespace Heart {
    /// <summary>State where the heart is immobile on the ground.</summary>
    public class Flung : State {
        public Flung(HeartStateMachine stateMachine) : base("Flung", stateMachine) { }

        
    }
}