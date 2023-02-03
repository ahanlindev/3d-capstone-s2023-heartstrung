using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

using BehaviorTree;

public class Patrol : Node
{
    private Transform _transform;

    private Transform[] _waypoints;

    private int _currentWaypoint = 0;

    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;

    // public Patrol(Transform transform, Transform[] waypoints) 
    // {
    //     _transform = transform;
    //     _waypoints = waypoints;
    // }

    // public override NodeState Evaluate()
    // {
    //     if (_waiting) {
    //         _waitCounter += Time.deltaTime;
    //         if (_waitCounter >= _waitTime) {
    //             _waiting = false;
    //         }
    //     } else {
    //         Transform wp = _waypoints[_currentWaypoint];
    //         if (Vector3.Distance(_transform.position, wp.position) < 0.01f) {
    //             _transform.position = wp.position;
    //             _waitCounter = 0f;
    //             _waiting = true;

    //             _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
    //         } else {
    //             _transform.position = Vector3.MoveTowards(_transform.position, wp.position, EnemyAI.speed * Time.deltaTime);
    //             _transform.LookAt(wp.position);
    //         }
    //     }
    //     state = NodeState.RUNNING;
    //     return state;
    // }

    private NavMeshAgent _navAgent;
    private float delta = 0.01f;
    
    public Patrol(NavMeshAgent navAgent, Transform[] waypoints) 
    {
        _navAgent = navAgent;
        _waypoints = waypoints;
    }


    public override NodeState Evaluate()
    {
        //pause at waypoint destination for a little bit
        // if (_waiting) {
        //     _waitCounter += Time.deltaTime;
        //     if (_waitCounter >= _waitTime) {
        //         _waiting = false;
        //     }
        // } else {
            Transform wp = _waypoints[_currentWaypoint];
            if (_navAgent.transform.position.x > wp.position.x - delta
            && _navAgent.transform.position.x < wp.position.x + delta
            && _navAgent.transform.position.z > wp.position.z - delta
            && _navAgent.transform.position.z < wp.position.z + delta) {
                // _navAgent.destination = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
            } else {
                _navAgent.destination = wp.position;
                _navAgent.transform.LookAt(wp.position);
            }
        // }
        state = NodeState.RUNNING;
        return state;
    }

}