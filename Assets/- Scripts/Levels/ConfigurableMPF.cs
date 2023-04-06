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
    /// Position of the platform at the end of the previous frame
    /// </summary>
    private Vector3 _lastPosition;
    
    /// <summary>
    /// Rigidbodies affected by the moving platform
    /// </summary>
    private HashSet<Rigidbody> _collidedBodies;
    
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
        UpdateAttachedBodies();
    }

    private void Awake() { _collidedBodies ??= new HashSet<Rigidbody>(); }
    
    private void OnEnable() {
        _lastPosition = transform.position;
        _collidedBodies ??= new HashSet<Rigidbody>();
        StartMovement();
    }

    private void OnDisable() {
        _collidedBodies = null;    
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        _collidedBodies ??= new HashSet<Rigidbody>();
        var temp = collision.collider.attachedRigidbody;
        if (temp != null) _collidedBodies.Add(temp);
        
        Transform playerTransform = collision.gameObject.transform;
        playerTransform.SetParent(transform.parent);
    }

    private void OnCollisionExit(Collision collision)
    {
        var temp = collision.collider.attachedRigidbody;
        if (temp != null) _collidedBodies.Remove(temp);   
        collision.gameObject.transform.SetParent(null);
        collision.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
    
    #endregion
    
    #region Private methods

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
    /// Move all attached rigidbodies and update last position
    /// </summary>
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
    
    #endregion
}
