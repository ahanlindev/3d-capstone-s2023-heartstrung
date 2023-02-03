using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerInRange : Node
{
    private Transform _transform;

    public CheckPlayerInRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object p = GetData("player");
        if (p == null) {
            state = NodeState.FAILURE;
            return state;
        }

        Transform player = (Transform)p;
        if (Vector3.Distance(_transform.position, player.position) <= EnemyAI.attackRange) {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }

}