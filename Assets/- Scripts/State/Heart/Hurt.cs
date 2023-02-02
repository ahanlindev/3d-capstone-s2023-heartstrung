using UnityEngine;

namespace Heart {
    /// <summary>State where the heart is immobile on the ground.</summary>
    public class Hurt : State {
        public Hurt(HeartStateMachine stateMachine) : base("Hurt", stateMachine) { }

        
    }
}