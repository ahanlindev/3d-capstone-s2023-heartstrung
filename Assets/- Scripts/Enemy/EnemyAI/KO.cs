using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class KO : Node
{
    private bool _isDead = false;
    private float _reviveCounter = 0f;
    private float _reviveTime = 10f;
    private UnityEngine.AI.NavMeshAgent _agent;
    private bool _attacked = false;

    public KO(UnityEngine.AI.NavMeshAgent agent, bool dead, bool attacked)
    {
        _agent = agent;
        _isDead = dead;
        _attacked = attacked;
    }   

    public override NodeState Evaluate()
    {
        //check if enemy is getting attacked,
        if (_attacked) {
            _attacked = false;
            //check if enemy is dead
            if (_isDead) {
                //if enemy is currently dead then stay dead for 10 seconds and then revive after
                _reviveCounter += Time.deltaTime;
                if (_reviveCounter >= _reviveTime) {
                    _isDead = false;
                    _agent.isStopped = false;
                    state = NodeState.FAILURE;
                    return state;
                }
            } else {
                //if enemy is not dead then successfully go to TakeHit node
                _reviveCounter = 0f;
                state = NodeState.SUCCESS;
                return state;
            }
        }
        //if not attacked, then go to next sequence
        state = NodeState.FAILURE;
        return state;
    }

}