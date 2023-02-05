using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckFOVRange : Node 
{
    private UnityEngine.AI.NavMeshAgent _agent;
    private static int _playerLayerMask = 1 << 6;
    public CheckFOVRange(UnityEngine.AI.NavMeshAgent agent)
    {
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        object player = GetData("player");
        if (player == null) {
            Collider[] colliders = Physics.OverlapSphere(_agent.transform.position, EnemyAI.fovRange, _playerLayerMask);
            if (colliders.Length > 0) {
                parent.parent.SetData("player", colliders[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }

}