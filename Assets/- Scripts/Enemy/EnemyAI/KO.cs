using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class KO : Node
{
    public KO()
    {

    }   

    public override NodeState Evaluate()
    {
        state = NodeState.RUNNING;
        return state;
    }
}