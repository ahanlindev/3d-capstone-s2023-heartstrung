using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerStateMachine : BaseStateMachine
    {
        // Possible States for this type of FSM
        [HideInInspector] public IdleState idleState;
        [HideInInspector] public MovingState movingState;
        [HideInInspector] public AttackingState attackingState;
        [HideInInspector] public ChargingState chargingState;
        [HideInInspector] public FlingingState flingingState;
        [HideInInspector] public HurtState hurtState;
        [HideInInspector] public DeadState deadState;

        // Shortcuts for input actions
        [HideInInspector] public InputAction _movementInput;
        [HideInInspector] public InputAction _attackInput;
        [HideInInspector] public InputAction _flingInput;
        [HideInInspector] public InputAction _jumpInput;

        // Input
        private PlayerInput _playerInput;

        // Player Controller
        private void Awake()
        {
            // construct each state
            idleState = new IdleState(this);
            movingState = new MovingState(this);
            attackingState = new AttackingState(this);
            chargingState = new ChargingState(this);
            flingingState = new FlingingState(this);
            hurtState = new HurtState(this);
            deadState = new DeadState(this);

            // initialize input
            _playerInput = new PlayerInput();
            _playerInput.Enable();

            _movementInput = _playerInput.Gameplay.Move;
            _attackInput = _playerInput.Gameplay.Attack;
            _flingInput = _playerInput.Gameplay.Fling;
            _jumpInput = _playerInput.Gameplay.Jump;
        }

        // Initial state for player should be idle
        protected override BaseState GetInitialState() => idleState;
    }
}