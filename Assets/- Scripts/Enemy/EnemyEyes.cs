using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class EnemyEyes : MonoBehaviour
{
    GameObject colliderGameObject;
    Vector3 colliderPosition;
    public event Action<Vector3> OnHeartSeenEvent;
    public event Action<Vector3> OnKittySeenEvent;
    public event Action OnHeartGoneEvent;
    public event Action OnKittyGoneEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Heart")) {
            Debug.Log("Dodger detected");
            colliderGameObject = other.gameObject;
            colliderPosition = colliderGameObject.transform.position;
            OnHeartSeenEvent?.Invoke(colliderPosition);
        } else if (other.CompareTag("Kitty")) {
            Debug.Log("kitty detected");
            colliderGameObject = other.gameObject;
            colliderPosition = colliderGameObject.transform.position;
            OnKittySeenEvent?.Invoke(colliderPosition);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Heart")) {
            OnHeartGoneEvent?.Invoke();
        } else if (other.CompareTag("Kitty")) {
            OnKittyGoneEvent?.Invoke();
        }
    }
}
 