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

        // Input
        private PlayerInput _playerInput;
        // Variables to act as shortcuts
        private InputAction _movementInput, _clawInput, _flingInput, _jumpMovement;  

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

            _movementInput = _playerInput.Gameplay.Movement;
            _clawInput = _playerInput.Gameplay.Claw;
            _flingInput = _playerInput.Gameplay.Fling;
            _jumpMovement = _playerInput.Gameplay.Jump;
        }

        protected override BaseState GetInitialState()
        {
            return idleState;
        }
    }
}