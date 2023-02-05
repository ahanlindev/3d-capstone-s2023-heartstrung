using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class KO : Node
{
    private bool _reviving = false;
    private float _reviveCounter = 0f;
    private float _reviveTime = 10f;
    private UnityEngine.AI.NavMeshAgent _agent;
    public KO(UnityEngine.AI.NavMeshAgent agent)
    {
        _agent = agent;
    }   

    public override NodeState Evaluate()
    {
        if (_reviving) {
            _reviveCounter += Time.deltaTime;
            if (_reviveCounter >= _reviveTime) {
                _reviving = false;
                _agent.isStopped = true;
            }
        } else {
            _reviveCounter = 0f;
            _reviving = true;
            _agent.isStopped = false;
        }
        state = NodeState.RUNNING;
        return state;
    }
}