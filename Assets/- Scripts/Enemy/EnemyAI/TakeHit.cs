using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TakeHit : Node
{
    public TakeHit()
    {
        
    }

    public override NodeState Evaluate()
    {
        state = NodeState.RUNNING;
        return state;
    }
}