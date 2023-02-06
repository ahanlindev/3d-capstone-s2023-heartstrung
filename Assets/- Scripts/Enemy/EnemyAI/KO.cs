using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class KO : Node
{
    private bool _reviving = false;

    private bool _isDead;
    private float _reviveCounter = 0f;
    private float _reviveTime = 10f;
    private UnityEngine.AI.NavMeshAgent _agent;
    private bool _attacked = false;

    public KO(UnityEngine.AI.NavMeshAgent agent, bool dead, bool attacked)
    {
        _agent = agent;
        _isDead = dead;
    }   

    public override NodeState Evaluate()
    {
        if (_isDead && _attacked) {
            _attacked = false;
            if (_reviving) {
            _reviveCounter += Time.deltaTime;
            if (_reviveCounter >= _reviveTime) {
                _reviving = false;
                _agent.isStopped = true;
                state = NodeState.FAILURE;
                return state;
            }
            } else {
                _reviveCounter = 0f;
                _reviving = true;
                _isDead = false;
                _agent.isStopped = false;
                state = NodeState.SUCCESS;
                return state;
            }
        }
        state = NodeState.FAILURE;
        return state;
    }

}