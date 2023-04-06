using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

public class ConfigurableMPF : MonoBehaviour
{

    #region Serialized Fields
    
    [FormerlySerializedAs("targets")]
    [Tooltip("Add/delete empty objects to this target list to set new waypoints.")]
    [SerializeField] private Transform[] _targets;

    [Tooltip("Speed of the moving platform, the duration from one waypoint to another is 1 / speed")]
    [SerializeField] private float _speed = 0.2f;

    [FormerlySerializedAs("hangTime")]
    [Tooltip("Time that the platform waits at a waypoint.")]
    [SerializeField] private float _hangTime = 3.0f;

    [FormerlySerializedAs("oneWay")]
    [Tooltip("If true, the platform stops after reaching the final waypoint of the list")]
    [SerializeField] private bool _oneWay;

    [Tooltip("If true, skips the initial rumble animation when enabled")] 
    [SerializeField] private bool _skipRumble;
    
    #endregion
    
    #region Private Fields & Properties
    
    /// <summary>
    /// Time taken to move from one waypoint to another
    /// </summary>
    private float moveDuration => 1f / _speed;

    /// <summary>
    /// Index of current waypoint
    /// </summary>
    private int _index;
    
    /// <summary>
    /// Timer that measures rest time and movement time between 2 waypoints
    /// </summary>
    private float _localTimer;

    /// <summary>
    /// Object at top level of scene with 1x1x1 scale, used to contain things that should move along with the platform
    /// </summary>
    private Transform _socket;
    
    /// <summary>
    /// If true, platform will no longer move after reaching its current target waypoint
    /// </summary>
    private bool _oneWayFinished;
    
    #endregion
    
    #region MonoBehaviour Methods
    
    // Update is called once per frame
    private void FixedUpdate()
    {   
        MoveTo();
        UpdateSocket();
    }

    private void OnEnable() {
        CreateSocket();
        StartMovement();
    }

    private void OnDisable() {
        Destroy(_socket);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // If collided with a rigidbody, make it a child of the socket object
        Rigidbody rbody = collision.collider.attachedRigidbody;
        Transform playerTransform = rbody.gameObject.transform;
        playerTransform.SetParent(_socket);
    }

    private void OnCollisionExit(Collision collision)
    {
        // If leaving object has a rigidbody and is attached to the socket, remove it
        Rigidbody rbody = collision.collider.attachedRigidbody;
        if (rbody && rbody.transform.IsChildOf(_socket))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
    
    #endregion
    
    #region Private methods
    
    /// <summary>
    /// Initialize socket. Create an empty game object at the root level of the scene.
    /// </summary>
    private void CreateSocket()
    {
        string socketName = $"{transform.parent.name}_MPFSocket";
        var socketObj = new GameObject(name: socketName);
        _socket = socketObj.transform;
    }
    
    private void StartMovement()
    {
        if (_skipRumble)
        {
            transform.DOMove(_targets[_index].position, moveDuration);
            return;
        }

        // Shake the platform to draw attention
        transform.DOShakePosition(duration: 2.0f, strength: new Vector3(.2f, 0, .2f), vibrato: 20)
            //Once shake finishes, begin movement
            .OnComplete(() => transform.DOMove(_targets[_index].position, moveDuration));
    }
    
    /// <summary>
    /// Timing and logic for movement and rest. Called in FixedUpdate.
    /// </summary>
    private void MoveTo()
    {
        if (_localTimer > 0)
        {
            _localTimer -= Time.fixedDeltaTime;
        } else if (!_oneWayFinished && transform.position == _targets[_index].position)
        {
            _index = (_index + 1) % _targets.Length;
            transform.DOMove(_targets[_index].position, moveDuration);
            _localTimer = _hangTime + moveDuration;

            // the final target wp, change the flag and stop
            if (_index == _targets.Length - 1 && _oneWay)
            {
                _oneWayFinished = true;
            }
        }        
    }
    
    /// <summary>
    /// Make sure the socket object stays synced with this one
    /// </summary>
    private void UpdateSocket()
    {
        if (!_socket) { return; }

        _socket.transform.position = transform.position;
    }
    
    #endregion
}
