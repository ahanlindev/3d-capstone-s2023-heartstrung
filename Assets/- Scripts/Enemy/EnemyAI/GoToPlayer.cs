using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class GoToPlayer : Node 
{
    private UnityEngine.AI.NavMeshAgent _agent;

    public GoToPlayer(UnityEngine.AI.NavMeshAgent agent)
    {
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        //find the player enemy found
        Transform player = (Transform)GetData("player");

        //go to players location
        if (Vector3.Distance(_agent.transform.position, player.position) > 0.5f) {
            _agent.destination = player.position;
            _agent.transform.LookAt(new Vector3(player.position.x, _agent.transform.position.y, player.position.z));
        }

        state = NodeState.RUNNING;
        return state;
    }

}