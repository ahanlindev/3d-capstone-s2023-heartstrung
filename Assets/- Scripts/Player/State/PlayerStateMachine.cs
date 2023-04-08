using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Runs player states and contains information required to maintain those states.</summary>
public class PlayerStateMachine : BaseStateMachine
{
    #region Public Events

    /// <summary>Event emitted when the player starts charging a fling.</summary>
    public Action ChargeFlingEvent;

    /// <summary>Event emitted when the player cancels charging a fling.</summary>
    public Action ChargeFlingCancelEvent;
    
    /// <summary>
    /// Event emitted when the player executes a fling. 
    /// First parameter is percentage fling power. 
    /// </summary>
    public Action<float> FlingEvent;

    /// <summary>
    ///Event emitted when the player is interrupted while in the flinging state
    /// </summary>
    public Action FlingInterruptedEvent;

    #endregion

    #region Possible States

    public Player.Idle idleState { get; private set; }
    public Player.Moving movingState { get; private set; }
    public Player.Attacking attackingState { get; private set; }
    public Player.Jumping jumpingState { get; private set; }
    public Player.Falling fallingState { get; private set; }
    public Player.Charging chargingState { get; private set; }
    public Player.Flinging flingingState { get; private set; }
    public Player.Hurt hurtState { get; private set; }
    public Player.Dead deadState { get; private set; }

    #endregion

    #region Public Properties
   
    // INPUT SHORTCUTS

    public InputAction movementInput { get; private set; }
    public InputAction attackInput { get; private set; }
    public InputAction flingInput { get; private set; }
    public InputAction jumpInput { get; private set; }

    // NON-SERIALIZED PROPERTIES

    public Rigidbody rbody { get; private set; }
    public Collider coll { get; private set; }
    public Claws claws { get; private set; }

    /// <summary>Trajectory renderer for charge and fling</summary>
    public TrajectoryRenderer trajectoryRenderer { get; private set; }

    /// <summary>
    /// Note that on the player, this is not truly a health count. 
    /// It is simply an interface to register hits from enemies.
    /// </summary>
    public Health hitTracker { get; private set; }

    /// <summary>Used by states to determine whether the hurt state can be entered</summary>
    public bool isInvincible { get; private set; }

    #endregion
    
    #region Serialized Public Properties

    [Header("Dodger")]

    [Tooltip("Heart script connected to this player")]
    [SerializeField] private HeartStateMachine _heart;
    public HeartStateMachine heart => _heart;

    [Tooltip("Player's max distance from Dodger in units")]
    [SerializeField] private float _maxTetherLength = 3f;
    public float maxTetherLength => _maxTetherLength;

    [Header("Mobility")]

    [Tooltip("Player's max speed in units/second")]
    [SerializeField] private float _moveSpeed = 2f;
    public float moveSpeed => _moveSpeed;

    [Tooltip("Movement speed multiplier when charging a fling")]
    [Range(0f,1f)][SerializeField] private float _chargingMovementMult = 0.15f;
    public float chargingMovementMult => _chargingMovementMult;

    [Tooltip("Movement speed multiplier when airborne")]
    [Range(0f,1f)][SerializeField] private float _airborneMovementMult = 0.75f;
    public float airborneMovementMult => _airborneMovementMult;

    [Tooltip("Player's jump power")]
    [SerializeField] private float _jumpPower = 5f;
    public float jumpPower => _jumpPower;

    [Tooltip("Time after becoming airborne before player starts to fall")]
    [SerializeField] private float _coyoteTime = .15f;
    public float coyoteTime => _coyoteTime;

    [Header("Fling")]

    [Tooltip("Percentage of fling power that will fill or lessen per second charging a fling")]
    [Range(0f, 1f)][SerializeField] private float _powerPerSecond = 0.55f;
    public float powerPerSecond => _powerPerSecond;

    [Tooltip("Minimum percentage of fling power that a fling can have")]
    [Range(0f, 1f)][SerializeField] private float _minPower = 0.35f;
    public float minPower => _minPower;

    [Tooltip("Maximum percentage of fling power that a fling can have")]
    [Range(0f, 1f)][SerializeField] private float _maxPower = 1f;
    public float maxPower => _maxPower;

    [Header("Misc.")]

    [Tooltip("Time in seconds that player will be unable to be hit after being hit once")]
    [Range(0f, 1f)][SerializeField] private float _invincibilityTime = 1f;
    public float invincibilityTime => _invincibilityTime;

    #endregion

    #region Private Fields
 
    private PlayerInput _playerInput;
    private Animator _anim;

    #endregion

    #region  MonoBehaviour Methods

    private void Awake()
    {
        // construct each state
        idleState = new Player.Idle(this);
        movingState = new Player.Moving(this);
        attackingState = new Player.Attacking(this);
        jumpingState = new Player.Jumping(this);
        fallingState = new Player.Falling(this);
        chargingState = new Player.Charging(this);
        flingingState = new Player.Flinging(this);
        hurtState = new Player.Hurt(this);
        deadState = new Player.Dead(this);

        // initialize input
        _playerInput = new PlayerInput();
        _playerInput.Enable();

        movementInput = _playerInput.Gameplay.Move;
        attackInput = _playerInput.Gameplay.Attack;
        flingInput = _playerInput.Gameplay.Fling;
        jumpInput = _playerInput.Gameplay.Jump;

        // initialize components
        rbody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        hitTracker = GetComponent<Health>();
        claws = GetComponentInChildren<Claws>();
        trajectoryRenderer = GetComponentInChildren<TrajectoryRenderer>();
        _anim = GetComponentInChildren<Animator>();

        InitTether();

        // validate non-guaranteed values
        if (!_anim) { Debug.LogError("PlayerStateMachine cannot find Animator component in children"); }
        if (!claws) { Debug.LogError("PlayerStateMachine cannot find Claws component in children"); }
        if (!trajectoryRenderer) { Debug.LogError("PlayerStateMachine cannot find TrajectoryRenderer component in children!"); }
        if (!hitTracker) { Debug.LogError("PlayerStateMachine cannot find a Health component!"); }
        if (!heart) { Debug.LogWarning("Player does not have a heart set!"); }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// If an animator parameter of the desired name exists, sets it to value. 
    /// If the desired parameter does not exists, a warning is logged to console. 
    /// </summary>
    /// <param name="paramName">Name of the animation parameter to update</param>
    /// <param name="value">Desired value for the animation parameter</param>
    public void SetAnimatorBool(string paramName, bool value)
    {
        if (AnimatorHasParam(paramName))
        {
            _anim.SetBool(paramName, value);
        }
        else
        {
            Debug.LogWarning($"State <color=blue>{paramName}</color> does not exist in Player's animator controller");
        }
    }

    /// <summary>
    /// If an animator parameter of the desired name exists, sets it to value. 
    /// If the desired parameter does not exists, a warning is logged to console. 
    /// </summary>
    /// <param name="paramName">Name of the animation parameter to update</param>
    /// <param name="value">Desired value for the animation parameter</param>
    public void SetAnimatorFloat(string paramName, float value)
    {
        if (AnimatorHasParam(paramName))
        {
            _anim.SetFloat(paramName, value);
        }
        else
        {
            Debug.LogWarning($"State <color=blue>{paramName}</color> does not exist in Player's animator controller");
        }
    }

    /// <summary>
    /// Get the length of the current animation clip, scaled by animator speed.
    ///</summary>
    /// <returns>
    /// The length in seconds of the current animation clip. 
    /// If no clip was found, returns -1.
    /// </returns>
    public float GetAnimatorClipLength() {
        AnimationClip clip = null; 
        for(int i = 0; i < _anim.layerCount; i++) {
            var clipArray = _anim.GetCurrentAnimatorClipInfo(i);
            if (clipArray.Any()) {
                clip = clipArray[0].clip;
                break; // exit loop early if a clip is found
            }
        }

        // scale clip length by runtime. If none exists, return -1
        return (clip) ? clip.length / _anim.speed : -1f;
    }
    
    /// <summary>Begins invincibility frames. Informs states that the hurt state should not be entered.</summary>
    public void StartInvincibility() => StartCoroutine(InvincibilityCoroutine());

    #endregion

    #region PRIVATE METHODS
    
    /// <summary>
    /// Initialization method used to set state on startup
    /// </summary>
    /// <returns>The state that this state machine should start in</returns>
    protected override BaseState GetInitialState() => idleState;

    /// <summary>
    /// Initialize the tether joint if it exists
    /// </summary>
    private void InitTether()
    {
        var tether = GetComponent<ConfigurableJoint>();
        if (!tether) { return; }

        SoftJointLimit tetherLimit = tether.linearLimit;
        tetherLimit.limit = maxTetherLength + 0.2f; // add delta for safety when working with limit
        tether.linearLimit = tetherLimit;
    }
    
    /// <summary>
    /// Helper method to check whether the state machine's 
    /// animator has a parameter with the desired name.
    /// </summary>
    /// <returns>True if a matching parameter is found, false otherwise.</returns>
    private bool AnimatorHasParam(string paramName)
    {
        return _anim.parameters.Any(param => param.name == paramName);
    }
    
    /// <summary>Performs the invincibility frame flicker</summary>
    private IEnumerator InvincibilityCoroutine() {
        // get all active renderers
        var renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        renderers.RemoveAll((r) => !r.enabled);

        // start invincibility
        isInvincible = true;

        // flicker renderers on and off until timer runs out
        float timer = 0;
        int frames = 0;
        while (timer < invincibilityTime) {
            yield return null;
            if (frames % 8 == 0) {
                renderers.ForEach((r) => r.enabled = !r.enabled);
            }
            frames++;
            timer += Time.deltaTime;
        }

        // cleanup at end of sequence
        isInvincible = false;
        renderers.ForEach((r) => r.enabled = true);
    }

    #endregion
}
