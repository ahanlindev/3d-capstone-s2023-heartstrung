using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Attack : Node
{
    private Transform _lastTarget;
    private float _attackTime = 3.0f;
    private float _attackCounter = 0f;

    private UnityEngine.AI.NavMeshAgent _agent;
    private EnemyClaw _enemyClaw;

    public Attack(UnityEngine.AI.NavMeshAgent agent, EnemyClaw enemyClaw)
    {
        _agent = agent;
        _enemyClaw = enemyClaw;
    }
    public override NodeState Evaluate()
    {
        //find the player to attack
        Transform player = (Transform)GetData("player");
        if (player != _lastTarget) {
            _lastTarget = player;
        }

        //perform attack after waiting attack cooldown 
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime) {
            //play attack animation
            _enemyClaw.Claw(0.66f);
            _attackCounter = 0f;
        }
        state = NodeState.RUNNING;
        return state;
    }
}