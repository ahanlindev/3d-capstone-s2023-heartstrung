using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

using BehaviorTree;

public class Patrol : Node
{
    private Transform[] _waypoints;

    private int _currentWaypoint = 0;

    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;
    private NavMeshAgent _navAgent;
    private float delta = 0.01f;
    
    public Patrol(NavMeshAgent navAgent, Transform[] waypoints) 
    {
        _navAgent = navAgent;
        _waypoints = waypoints;
    }


    public override NodeState Evaluate()
    {
        // pause at waypoint destination for a little bit
        if (_waiting) {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime) {
                _waiting = false;
            }
        } else {
            Transform wp = _waypoints[_currentWaypoint];
            var agentPos = new Vector3(_navAgent.transform.position.x, 0, _navAgent.transform.position.z);
            var wpPos = wp.position;
            agentPos.y = 0;
            wpPos.y = 0;
            if (Vector3.Distance(agentPos, wpPos) <= delta) {
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
            } else {
                _navAgent.destination = wp.position;
                _navAgent.transform.LookAt(new Vector3(wp.position.x, _navAgent.transform.position.y, wp.position.z));
            }
        }
        state = NodeState.RUNNING;
        return state;
    }

}