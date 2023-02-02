using UnityEngine;

/// <summary>Runs heart states and contains information required to maintain those states.</summary>
public class HeartStateMachine : BaseStateMachine {
    // Emitted events
    public event System.Action LandedEvent;
    
    // Possible States
    public Heart.Idle idleState { get; private set; }
    public Heart.Flung flungState { get; private set; }
    public Heart.Falling fallingState { get; private set; }
    public Heart.Hurt hurtState { get; private set; }
    public Heart.Dead deadState { get; private set; }

    // Necessary components
    public Rigidbody rbody { get; private set;}
    public Collider coll { get; private set;}
    public Vector3 currentDestination { get; private set;} // TODO this might belong in Heart.Flung class
    public float totalFlingAngle { get; private set;}
    public float initialFlingRadius { get; private set;}

    // Inspector-visible values
    [Tooltip("PlayerStateMachine instance for the player attached to this object")]
    [SerializeField] private PlayerStateMachine _player;
    public PlayerStateMachine player {get => _player; private set => _player = value; }
    
    [Tooltip("Linear speed along the arc that the heart will fly through the air in units per second")]
    [SerializeField] private float _flingSpeed = 15.0f;
    public float flingSpeed { get => _flingSpeed; private set => _flingSpeed = value; }
    
    // private fields
    private Animator anim;

    private void Awake() {
        // construct each state
        idleState = new Heart.Idle(this);
        flungState = new Heart.Flung(this);
        fallingState = new Heart.Falling(this);
        hurtState = new Heart.Hurt(this);
        deadState = new Heart.Dead(this);

        // initialize components
        rbody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        if (!player) {
            Debug.LogError($"HeartStateMachine on <color=yellow>{gameObject.name}</color> has no connected PlayerStateMachine!");
        }
    }
}