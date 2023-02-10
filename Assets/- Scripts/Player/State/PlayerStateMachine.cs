using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Runs player states and contains information required to maintain those states.</summary>
public class PlayerStateMachine : BaseStateMachine
{
    // Emitted events -----------------------------------------------------
    
    /// <summary>Event emitted when the player starts charging a fling.</summary>
    public Action ChargeFlingEvent;

    /// <summary>
    /// Event emitted when the player executes a fling. 
    /// First parameter is percentage fling power. 
    /// </summary>
    public Action<float> FlingEvent;

    /// <summary>
    ///Event emitted when the player is interrupted while in the flinging state
    /// </summary>
    public Action FlingInterruptedEvent;

    // Possible States ----------------------------------------------------
    public Player.Idle idleState { get; private set; }
    public Player.Moving movingState { get; private set; }
    public Player.Attacking attackingState { get; private set; }
    public Player.Jumping jumpingState { get; private set; }
    public Player.Falling fallingState { get; private set; }
    public Player.Charging chargingState { get; private set; }
    public Player.Flinging flingingState { get; private set; }
    public Player.Hurt hurtState { get; private set; }
    public Player.Dead deadState { get; private set; }

    // Shortcuts for input actions ----------------------------------------
    public InputAction movementInput { get; private set; }
    public InputAction attackInput { get; private set; }
    public InputAction flingInput { get; private set; }
    public InputAction jumpInput { get; private set; }

    // Other necessary components -----------------------------------------
    public Rigidbody rbody { get; private set; }
    public Collider coll { get; private set; }
    public Claws claws { get; private set; }

    /// <summary>
    /// Note that on the player, this is not truly a health count. 
    /// It is simply an interface to register hits from enemies.
    /// </summary>
    public Health hitTracker { get; private set; }

    // Inspector-visible values -------------------------------------------
    [Tooltip("Heart connected to this player")]
    [SerializeField] private HeartStateMachine _heart;
    public HeartStateMachine heart { get => _heart; private set => _heart = value; }

    // TODO this is sloppy. Refactor soon.
    [Tooltip("Amount of time in seconds that the player will be in the attack state")]
    [SerializeField] private float _attackTime = 0.5f;
    public float attackTime { get => _attackTime; private set => _attackTime = value; }

    // TODO this is sloppy. Refactor soon.
    [Tooltip("Amount of time in seconds that the player will be in the hurt state")]
    [SerializeField] private float _hurtTime = 0.5f;
    public float hurtTime { get => _hurtTime; private set => _hurtTime = value; }

    [Tooltip("Player's max speed in units/second")]
    [SerializeField] private float _moveSpeed = 2f;
    public float moveSpeed { get => _moveSpeed; private set => _moveSpeed = value; }

    [Tooltip("Player's jump power")]
    [SerializeField] private float _jumpPower = 5f;
    public float jumpPower { get => _jumpPower; private set => _jumpPower = value; }

    [Tooltip("Player's max distance from Dodger in units")]
    [SerializeField] private float _maxTetherLength = 3f;
    public float maxTetherLength { get => _maxTetherLength; private set => _maxTetherLength = value; }

    [Tooltip("Percentage of fling power that will fill or lessen per second charging a fling")]
    [Range(0f, 1f)][SerializeField] private float _powerPerSecond = 0.85f;
    public float powerPerSecond { get => _powerPerSecond; private set => _powerPerSecond = value; }

    [Tooltip("Minimum percentage of fling power that a fling can have")]
    [Range(0f, 1f)][SerializeField] private float _minPower = 0.15f;
    public float minPower { get => _minPower; private set => _minPower = value; }

    [Tooltip("Maximum percentage of fling power that a fling can have")]
    [Range(0f, 1f)][SerializeField] private float _maxPower = 1f;
    public float maxPower { get => _maxPower; private set => _maxPower = value; }

    // Private fields ----------------------------------------------------
    private PlayerInput _playerInput;
    private Animator anim;

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

        anim = GetComponentInChildren<Animator>();

        // initialize tether (TODO this should either be a method or done elsewhere)
        var tether = GetComponent<ConfigurableJoint>();
        if (tether)
        {
            var tetherLimit = tether.linearLimit;
            tetherLimit.limit = maxTetherLength;
            tether.linearLimit = tetherLimit;
        }

        // validate non-guaranteed values
        if (!anim) { Debug.LogError("PlayerStateMachine cannot find Animator component in children"); }
        if (!claws) { Debug.LogError("PlayerStateMachine cannot find Claws component in children"); }
        if (!heart) { Debug.LogWarning("Player does not have a heart set!"); }
    }

    // Initial state for player should be idle
    protected override BaseState GetInitialState() => idleState;

    /// <summary>
    /// If an animator parameter of the desired name exists, sets it to value. 
    /// If the desired parameter does not exists, a warning is logged to console. 
    /// </summary>
    /// <param name="name">Name of the animation parameter to update</param>
    /// <param name="value">Desired value for the animation parameter</param>
    public void SetAnimatorBool(string name, bool value)
    {
        if (AnimatorHasParam(name))
        {
            anim.SetBool(name, value);
        }
        else
        {
            Debug.LogWarning($"State <color=blue>{name}</color> does not exist in Player's animator controller");
        }
    }

    /// <summary>
    /// Helper method to check whether the state machine's 
    /// animator has a parameter with the desired name.
    /// </summary>
    /// <returns>True if a matching parameter is found, false otherwise.</returns>
    private bool AnimatorHasParam(string paramName)
    {
        var matchingParams = anim.parameters.Where((param) => param.name == paramName);
        return matchingParams.Count() > 0;
    }
}
