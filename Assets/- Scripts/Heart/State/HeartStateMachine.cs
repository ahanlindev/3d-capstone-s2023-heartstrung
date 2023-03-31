using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Runs heart states and contains information required to maintain those states.</summary>
public class HeartStateMachine : BaseStateMachine
{
    // Emitted events ----------------------------------

    /// <summary>Notifies listeners when landing after being flung</summary>
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

    /// <summary>Used by states to determine whether the hurt state can be entered</summary>
    public bool isInvincible { get; private set; }

    // Inspector-visible values
    [Tooltip("PlayerStateMachine instance for the player attached to this object")]
    [SerializeField] private PlayerStateMachine _player;
    public PlayerStateMachine player => _player;

    [Tooltip("Speed along the arc that the heart will fly through the air in degrees per second")]
    [SerializeField] private float _flingSpeed = 180.0f;
    public float flingSpeed => _flingSpeed;

    [Tooltip("Time in seconds that the heart will be in the hurt state when hurt")]
    [SerializeField] private float _hurtTime = 0.5f;
    public float hurtTime => _hurtTime;

    [Tooltip("Time in seconds that player will be unable to be hit after being hit once")]
    [Range(0f, 1f)][SerializeField] private float _invincibilityTime = 1f;
    public float invincibilityTime => _invincibilityTime;

    [Tooltip("Time in seconds that the heart can be in the falling state before the player is reset")]
    [SerializeField] private float _fallResetTime = 3.0f;
    public float fallResetTime => _fallResetTime;

    // Private Fields --------------------------------------------
    private Animator _anim;

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

        _anim = GetComponentInChildren<Animator>();

        if (!player)
        {
            Debug.LogError($"HeartStateMachine on <color=yellow>{gameObject.name}</color> has no connected PlayerStateMachine!");
        }
        if (!_anim)
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
            Debug.LogWarning($"State <color=blue>{paramName}</color> does not exist in Heart's animator controller");
        }
    }

    /// <summary>Begins invincibility frames. Informs states that the hurt state should not be entered.</summary>
    public void StartInvincibility() => StartCoroutine(InvincibilityCoroutine());

    /// <summary>
    /// Helper method to check whether the state machine's 
    /// animator has a parameter with the desired name.
    /// </summary>
    /// <param name="paramName">Name of the checked animator parameter</param>
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

}