using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class GoToPlayer : Node 
{
    private Transform _transform;

    private UnityEngine.AI.NavMeshAgent _agent;

    // public GoToPlayer(Transform transform)
    // {
    //     _transform = transform;
    // }

    public GoToPlayer(UnityEngine.AI.NavMeshAgent agent)
    {
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        Transform player = (Transform)GetData("player");

        if (Vector3.Distance(_agent.transform.position, player.position) > 0.01f) {
            _agent.transform.position = Vector3.MoveTowards(_agent.transform.position, player.position, EnemyAI.speed * Time.deltaTime);
            _agent.transform.LookAt(player.position);
        }

        state = NodeState.RUNNING;
        return state;
    }

    // public override NodeState Evaluate()
    // {
    //     Transform player = (Transform)GetData("player");

    //     if (Vector3.Distance(_transform.position, player.position) > 0.01f) {
    //         _transform.position = Vector3.MoveTowards(_transform.position, player.position, EnemyAI.speed * Time.deltaTime);
    //         _transform.LookAt(player.position);
    //     }

    //     state = NodeState.RUNNING;
    //     return state;
    // }
}