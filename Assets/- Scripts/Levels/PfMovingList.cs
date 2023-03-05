using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PfMovingList : MonoBehaviour
{

    [Tooltip("Add/delete empty objects to the target list to set new waypoints.")]
    [SerializeField] public Transform[] targets;

    [Tooltip("Speed of movingpf, the duration is 1 / speed")]
    [SerializeField] public float _speed = 0.2f;

    [Tooltip("Time for the platform waiting at one waypoint.")]
    [SerializeField] public float hangTime = 3.0f;

    [Tooltip("If it stop after reach the final wp of the list")]
    [SerializeField] public bool oneWay;

    private int _index = 0;
    private float _localTimer = 0;
   
    private Vector3 _lastPosition;
    private HashSet<Rigidbody> _collidedBodies;
    
    private bool _moved;


    // Update is called once per frame
    void FixedUpdate()
    {   
        moveTo();
        UpdateAttachedBodies();
    }

    private void Awake() {
        if (collidedBodies == null) collidedBodies = new HashSet<Rigidbody>();
    }
    
    private void OnEnable() {
        _lastPosition = transform.position;
        if (_collidedBodies == null) _collidedBodies = new HashSet<Rigidbody>();
        transform.DOMove(targets[_index].position, 1 / _speed);
    }

    private void OnDisable() {
        _collidedBodies = null;    
    }

    void moveTo()
    {
        
        if (_localTimer > 0)
        {
            _localTimer -= Time.fixedDeltaTime;
        } else if (!_moved && transform.position == targets[_index].position)
        {
            _index = (_index + 1) % targets.Length;
            transform.DOMove(targets[_index].position, 1 / _speed);
            _localTimer = hangTime + 1 / _speed;

            // the final target wp, change the flag and stop
            if (_index == targets.Length - 1 && oneWay)
            {
                _moved = true;
            }
            
        }        
    }

    private void UpdateAttachedBodies() 
    {
        if (_collidedBodies.Count != 0)
        {
            Vector3 offset = transform.position - _lastPosition;
            foreach (Rigidbody attached in _collidedBodies)
            {
                attached.MovePosition(attached.transform.position + offset);
            }
        }
        _lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (_collidedBodies == null) _collidedBodies = new HashSet<Rigidbody>();
        var temp = collision.collider.attachedRigidbody;
        if (temp != null) _collidedBodies.Add(temp);
    }

    private void OnCollisionExit(Collision collision)
    {
        var temp = collision.collider.attachedRigidbody;
        if (temp != null) _collidedBodies.Remove(temp);
    }
}
