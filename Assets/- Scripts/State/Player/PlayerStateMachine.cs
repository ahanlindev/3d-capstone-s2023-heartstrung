using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerStateMachine : BaseStateMachine
    {
        // Possible States
        public IdleState idleState { get; private set; }
        public MovingState movingState { get; private set; }
        public AttackingState attackingState { get; private set; }
        public JumpingState jumpingState { get; private set; }
        public FallingState fallingState { get; private set; }
        public ChargingState chargingState { get; private set; }
        public FlingingState flingingState { get; private set; }
        public HurtState hurtState { get; private set; }
        public DeadState deadState { get; private set; }

        // Shortcuts for input actions
        public InputAction movementInput { get; private set; }
        public InputAction attackInput { get; private set; }
        public InputAction flingInput { get; private set; }
        public InputAction jumpInput { get; private set; }

        // Other necessary components
        public Rigidbody rbody { get; private set; }
        public Collider coll { get; private set; }
        public Claws claws { get; private set; }

        // Inspector-set values
        [Tooltip("Heart connected to this player")]
        [SerializeField] private Heart _heart;
        public Heart heart { get => _heart; private set => _heart = value; }

        // TODO this is sloppy. Refactor soon.
        [Tooltip("Amount of time in seconds that the player will be in the attack state")]
        [SerializeField] private float _attackTime = 0.5f;
        public float attackTime { get => _attackTime; private set => _attackTime = value; }

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

        // Private fields
        private PlayerInput _playerInput;
        private Animator anim;

        // Player Controller
        private void Awake()
        {
            // construct each state
            idleState = new IdleState(this);
            movingState = new MovingState(this);
            attackingState = new AttackingState(this);
            jumpingState = new JumpingState(this);
            fallingState = new FallingState(this);
            chargingState = new ChargingState(this);
            flingingState = new FlingingState(this);
            hurtState = new HurtState(this);
            deadState = new DeadState(this);

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
            anim = GetComponentInChildren<Animator>();
            claws = GetComponentInChildren<Claws>();

            // validate non-guaranteed values
            if (!anim) {Debug.LogError("PlayerStateMachine cannot find Animator component in children"); }
            if (!claws) {Debug.LogError("PlayerStateMachine cannot find Claws component in children"); }
            if (!heart) { Debug.LogError("PlayerStateMachine has no Heart set!"); }
        }

        // Initial state for player should be idle
        protected override BaseState GetInitialState() => idleState;

        /// <summary>
        /// If an animator parameter of the desired name exists, sets it to value. 
        /// If the desired parameter does not exists, a warning is logged to console. 
        /// </summary>
        /// <param name="name">Name of the animation parameter to update</param>
        /// <param name="value">Desired value for the animation parameter</param>
        public void SetAnimatorBool(string name, bool value) {
            if (AnimatorHasParam(name)) {
                anim.SetBool(name, value);
            } else {
                Debug.LogWarning($"State <color=blue>{name}</color> does not exist in Player's animator controller");
            }
        }

        /// <summary>
        /// Helper method to check whether the state machine's 
        /// animator has a parameter with the desired name.
        /// </summary>
        /// <returns>True if a matching parameter is found, false otherwise.</returns>
        private bool AnimatorHasParam(string paramName) {
            var matchingParams = anim.parameters.Where((param) => param.name == paramName);
            return matchingParams.Count() > 0;
        }
    }
}