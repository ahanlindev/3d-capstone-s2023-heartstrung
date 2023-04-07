using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{



    [Tooltip("Add/delete empty objects to the target list to set new waypoints.")]
    [SerializeField] public Transform[] targets;

    [Tooltip("Speed of movingpf, the duration is 1 / speed")]
    [SerializeField] public float speed = 0.2f;

    [Tooltip("Time for the platform waiting at one waypoint.")]
    [SerializeField] public float hangTime = 3.0f;

    public int triggeringObjNum = 2;

    private float _localTimer = 0;

    private Vector3 _lastPosition;
    private HashSet<Rigidbody> _collidedBodies;
    private bool _standOn;
    private int _index = 0;
   


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
        if (transform.position == targets[_index].position)
        {
            _index = Mathf.Min(_index + 1, targets.Length - 1);
        }

        if (_localTimer > 0)
        {
            _localTimer -= Time.fixedDeltaTime;
        }
        else if (_standOn && transform.position != targets[_index].position)
        {
            transform.DOMove(targets[_index].position, 1 / speed);
            _localTimer = hangTime + 1 / speed;

        } else if (!_standOn && transform.position != targets[0].position)
        {
            transform.DOMove(targets[0].position, 1 / speed);
            _localTimer = hangTime + 1 / speed;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (_collidedBodies == null) _collidedBodies = new HashSet<Rigidbody>();
        var temp = collision.collider.attachedRigidbody;
        _collidedBodies.Add(temp);
        collision.gameObject.transform.SetParent(transform);
        if (_collidedBodies.Count == triggeringObjNum)
            _standOn = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        var temp = collision.collider.attachedRigidbody;
        if (temp != null) _collidedBodies.Remove(temp);
        collision.gameObject.transform.SetParent(null);
        collision.gameObject.transform.localScale = new Vector3(1, 1, 1);
        if (_collidedBodies.Count != triggeringObjNum)
            _standOn = false;
    }
}
