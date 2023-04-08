using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class KO : Node
{
    private float _reviveCounter = 0f;
    private float _reviveTime = 10f;
    private UnityEngine.AI.NavMeshAgent _agent;
    private Health _health;
    private static int _clawLayerMask = 1 << 9;
    private bool _attacked = false;
    private float _healToFullHP = 100.0f;

    public KO(UnityEngine.AI.NavMeshAgent agent, Health health)
    {
        _agent = agent;
        _health = health;
    }   

    public override NodeState Evaluate()
    {
        bool _isDead = _health.currentHealth <= 0;
        //check if enemy is dead
        if (_isDead) {
            //if enemy is currently dead then stay dead for 10 seconds and then revive after
            _reviveCounter += Time.deltaTime;
            if (_reviveCounter >= _reviveTime) {
                _agent.isStopped = false;
                _health.ChangeHealth(_healToFullHP);
                state = NodeState.FAILURE;
                return state;
            }
        }        
        // look for Kitty's claws
        Collider[] KittyClaw = Physics.OverlapSphere(_agent.transform.position, EnemyAI.attackRange, _clawLayerMask);
        _attacked = false;
        // check if enemy is getting clawed right now
         if (KittyClaw.Length > 0) {
            _attacked = true;
         }
        if (_attacked) {
            _attacked = false;
            //if enemy is not dead then successfully go to TakeHit node
            _reviveCounter = 0f;
            state = NodeState.SUCCESS;
            return state;
        }
        //if not attacked, then go to next sequence
        state = NodeState.FAILURE;
        return state;
    }

}