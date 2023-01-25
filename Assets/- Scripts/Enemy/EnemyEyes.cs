using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class EnemyEyes : MonoBehaviour
{
    public float TargetDetectionDistance;

    private RaycastHit _hitInfo;


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
        bool found = false;
        bool total_found = false;
        var pov_origin = Quaternion.AngleAxis(60.0f, transform.up) * transform.forward;
        for (int i = 0; i < 8; i ++) {
            // set vars
            found = Physics.Raycast(transform.position, pov_origin,
                out _hitInfo, TargetDetectionDistance);
            total_found |= found;
            pov_origin = Quaternion.AngleAxis(-15.0f, transform.up) * pov_origin;
            if (found) {
                if (_hitInfo.transform.CompareTag("Heart")) {
                    Debug.Log("heart detected");
                    OnHeartSeenEvent?.Invoke(_hitInfo.transform.position);
                } else if (_hitInfo.transform.CompareTag("Player")) {
                    Debug.Log("player detected");
                    OnPlayerSeenEvent?.Invoke(_hitInfo.transform.position);
                }
            }
        }
        if (!total_found) {
            OnHeartGoneEvent?.Invoke();
            OnPlayerGoneEvent?.Invoke();
        }
    }
}
 