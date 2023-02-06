using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TakeHit : Node
{   
    private Health _health;
    private bool _isDead;

    public TakeHit(Health health)
    {
        _health = health;
    }

    public override NodeState Evaluate()
    {
        //this needs to take in the event that is called when Kitty attacks
        /*
            if (not dead and attacked by kitty)
                take dmg - call changeHealth
                if(health is lower than or equal to 0
        */  

        // if health is not zero yet then take damage
        // if (!_isDead) {
        //     _health.ChangeHealth(-10.0f);
        // }
        // Debug.LogError("Enemy is taking damage");

        state = NodeState.RUNNING;
        return state;
    }
}