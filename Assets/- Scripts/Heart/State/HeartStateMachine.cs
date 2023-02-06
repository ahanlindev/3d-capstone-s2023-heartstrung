using System.Linq;
using UnityEngine;

/// <summary>Runs heart states and contains information required to maintain those states.</summary>
public class HeartStateMachine : BaseStateMachine
{
    // Emitted events ----------------------------------

    /// <summary>Notifies listeners when landing after being flung</summary> // TODO might be useful to have this go off when landing at all?
    public System.Action LandedEvent;

    /// <summary>Used to inform current state about an entered collision</summary>
    public event System.Action<Collision> CollisionEnterEvent;

    // Possible States ----------------------------------
    public Heart.Idle idleState { get; private set; }
    public Heart.Flung flungState { get; private set; }
    public Heart.Falling fallingState { get; private set; }
    public Heart.Hurt hurtState { get; private set; }
    public Heart.Dead deadState { get; private set; }

    // Necessary properties ----------------------------------
    public Rigidbody rbody { get; private set; }
    public Collider coll { get; private set; }
    public Health health { get; private set; }

    /// <summary>Percentage power that the player has thrown the heart. Updated by the player. </summary>
    public float flingPower { get; private set; }

    /// <summary>True if the heart is in a flingable state, false otherwise. Updated by states.</summary>
    public bool canBeFlung { get; set; }


    // Inspector-visible values
    [Tooltip("PlayerStateMachine instance for the player attached to this object")]
    [SerializeField] private PlayerStateMachine _player;
    public PlayerStateMachine player { get => _player; private set => _player = value; }

    [Tooltip("Speed along the arc that the heart will fly through the air in degrees per second")]
    [SerializeField] private float _flingSpeed = 180.0f;
    public float flingSpeed { get => _flingSpeed; private set => _flingSpeed = value; }

    [Tooltip("Time in seconds that the heart will be in the hurt state when hurt")]
    [SerializeField] private float _hurtTime = 0.5f;
    public float hurtTime { get => _hurtTime; private set => _hurtTime = value; }

    // Private Fields --------------------------------------------
    private Animator anim;

    private void Awake()
    {
        // construct each state
        idleState = new Heart.Idle(this);
        flungState = new Heart.Flung(this);
        fallingState = new Heart.Falling(this);
        hurtState = new Heart.Hurt(this);
        deadState = new Heart.Dead(this);

        // initialize components
        rbody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        health = GetComponent<Health>();

        anim = GetComponentInChildren<Animator>();

        if (!player)
        {
            Debug.LogError($"HeartStateMachine on <color=yellow>{gameObject.name}</color> has no connected PlayerStateMachine!");
        }
        if (!anim)
        {
            Debug.LogError($"HeartStateMachine on <color=yellow>{gameObject.name}</color> cannot find animator component in self or children");
        }
    }

    private void OnEnable()
    {
        player.FlingEvent += UpdateFlingPower;
    }

    private void OnDisable()
    {
        player.FlingEvent -= UpdateFlingPower;
    }

    // used to inform state about collision information
    private void OnCollisionEnter(Collision other)
    {
        CollisionEnterEvent?.Invoke(other);
    }

    protected override BaseState GetInitialState() => idleState;

    // Event handlers ------------------------------------------

    private void UpdateFlingPower(float value)
    {
        flingPower = value;
    }

    // Helper methods ------------------------------------------

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
            Debug.LogWarning($"State <color=blue>{name}</color> does not exist in Heart's animator controller");
        }
    }

    /// <summary>
    /// Helper method to check whether the state machine's 
    /// animator has a parameter with the desired name.
    /// </summary>
    /// <param name="paramName">Name of the checked animator parameter</param>
    /// <returns>True if a matching parameter is found, false otherwise.</returns>
    private bool AnimatorHasParam(string paramName)
    {
        var matchingParams = anim.parameters.Where((param) => param.name == paramName);
        return matchingParams.Count() > 0;
    }
}