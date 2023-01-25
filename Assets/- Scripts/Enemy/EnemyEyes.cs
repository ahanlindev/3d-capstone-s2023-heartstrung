using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class EnemyEyes : MonoBehaviour
{
    public float TargetDetectionDistance;

    private RaycastHit _hitInfo;
    private bool _enemyDetected = false;


    public event Action<Vector3> OnHeartSeenEvent;
    public event Action<Vector3> OnPlayerSeenEvent;
    public event Action OnHeartGoneEvent;
    public event Action OnPlayerGoneEvent;


    void Update() 
    {
        CheckForPlayerInLineOfSight();
    }

    public void CheckForPlayerInLineOfSight() 
    {
        _enemyDetected = Physics.Raycast(transform.position, transform.forward,
            out _hitInfo, TargetDetectionDistance);
        if (_enemyDetected) {
            if (_hitInfo.transform.CompareTag("Heart")) {
                Debug.Log("heart detected");
                OnHeartSeenEvent?.Invoke(_hitInfo.transform.position);
            } else if (_hitInfo.transform.CompareTag("Player")) {
                Debug.Log("player detected");
                OnPlayerSeenEvent?.Invoke(_hitInfo.transform.position);
            }
        } else {
            OnHeartGoneEvent?.Invoke();
            OnPlayerGoneEvent?.Invoke();
        }
    }
}
 