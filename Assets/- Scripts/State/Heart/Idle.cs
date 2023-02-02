using UnityEngine;

namespace Heart {
    /// <summary>State where the heart is immobile on the ground.</summary>
    public class Idle : State {
        public Idle(HeartStateMachine stateMachine) : base("Idle", stateMachine) { }

        
    }
}