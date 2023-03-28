using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{

    [Tooltip("Origininal elevator WP")]
    [SerializeField] public Transform startWP;
    [Tooltip("Target elevator WP")]
    [SerializeField] public Transform targetWP;

    [Tooltip("Speed of movingpf, the duration is 1 / speed")]
    [SerializeField] public float _speed = 0.2f;

    [Tooltip("Time for the platform waiting at one waypoint.")]
    [SerializeField] public float hangTime = 3.0f;


    private float _localTimer = 0;

    private Vector3 _lastPosition;
    private HashSet<Rigidbody> _collidedBodies;
    private bool _standOn;
   


    // Update is called once per frame
    void FixedUpdate()
    {
        moveTo();
        //UpdateAttachedBodies();
    }

    private void Awake()
    {
        if (_collidedBodies == null) _collidedBodies = new HashSet<Rigidbody>();
    }

 

    void moveTo()
    {

        if (_localTimer > 0)
        {
            _localTimer -= Time.fixedDeltaTime;
        }
        else if (_standOn && transform.position != targetWP.position)
        {
            transform.DOMove(targetWP.position, 1 / _speed);
            _localTimer = hangTime + 1 / _speed;

        } else if (!_standOn && transform.position != startWP.position)
        {
            transform.DOMove(startWP.position, 1 / _speed);
            _localTimer = hangTime + 1 / _speed;
        }
    }

    //private void UpdateAttachedBodies()
    //{
    //    if (_collidedBodies.Count != 0)
    //    {
    //        Vector3 offset = transform.position - _lastPosition;
    //        foreach (Rigidbody attached in _collidedBodies)
    //        {
    //            attached.MovePosition(attached.transform.position + offset);
    //        }
    //    }
    //    _lastPosition = transform.position;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (_collidedBodies == null) _collidedBodies = new HashSet<Rigidbody>();
        var temp = collision.collider.attachedRigidbody;
        _collidedBodies.Add(temp);
        collision.gameObject.transform.SetParent(transform);
        Debug.Log(_collidedBodies.Count);
        if (_collidedBodies.Count == 2)
            _standOn = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        var temp = collision.collider.attachedRigidbody;
        if (temp != null) _collidedBodies.Remove(temp);
        collision.gameObject.transform.SetParent(null);
        collision.gameObject.transform.localScale = new Vector3(1, 1, 1);
        if (_collidedBodies.Count != 2)
            _standOn = false;
    }
}
