using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Attack : Node
{
    private Transform _lastTarget;
    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    private UnityEngine.AI.NavMeshAgent _agent;

    public Attack(UnityEngine.AI.NavMeshAgent agent)
    {
        _agent = agent;
    }
    public override NodeState Evaluate()
    {
        Transform player = (Transform)GetData("player");
        if (player != _lastTarget) {
            _lastTarget = player;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime) {
            // bool playerIsDead = _enemyManager.TakeHit();
            // if (playerIsDead) {
            //     ClearData("player");
            // } else {
            //     _attackCounter = 0f;
            // }
        }

    // public override NodeState Evaluate()
    // {
    //     Transform player = (Transform)GetData("player");
    //     if (player != _lastTarget) {
    //         _lastTarget = player;
    //     }

    //     _attackCounter += Time.deltaTime;
    //     if (_attackCounter >= _attackTime) {
    //         // bool playerIsDead = _enemyManager.TakeHit();
    //         // if (playerIsDead) {
    //         //     ClearData("player");
    //         // } else {
    //         //     _attackCounter = 0f;
    //         // }

        state = NodeState.RUNNING;
        return state;
    }
}