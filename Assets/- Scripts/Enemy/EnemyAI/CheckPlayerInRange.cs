using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerInRange : Node
{
    private UnityEngine.AI.NavMeshAgent _agent;

    public CheckPlayerInRange(UnityEngine.AI.NavMeshAgent agent)
    {
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        object p = GetData("player");
        if (p == null) {
            state = NodeState.FAILURE;
            return state;
        }

        Transform player = (Transform)p;
        if (Vector3.Distance(_agent.transform.position, player.position) <= EnemyAI.attackRange) {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }

}