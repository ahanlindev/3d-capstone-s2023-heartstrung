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
        //play hurt animation here
         Debug.LogError("Enemy is taking damage");

        state = NodeState.RUNNING;
        return state;
    }
}