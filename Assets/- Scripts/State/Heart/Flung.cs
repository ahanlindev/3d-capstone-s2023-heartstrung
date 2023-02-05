using UnityEngine;
using DG.Tweening;

namespace Heart {
    /// <summary>State where the heart is flying through the air towards a destination.</summary>
    public class Flung : State {
        public Flung(HeartStateMachine stateMachine) : base("Flung", stateMachine) { }
        
        private Transform _playerTf;
        private Transform _tf;
        private Vector3 _destination;
        private float _currentRadius;
        private float _finalRadius;
        private float _totalAngleToDest;
        private float _cumulativeAngle = 0f;

        public override void Enter()
        {
            base.Enter();
            _playerTf = _stateMachine.player.transform;
            _tf = _stateMachine.transform; 

            // get player->me and player->dest vectors
            Vector3 playerToMe = _tf.position - _playerTf.position;
            Vector3 playerToDest = _playerTf.forward * playerToMe.magnitude * _stateMachine.flingPower;

            // Lin-alg wizardry to face upward, with stem facing player, so rotation looks nicer
            Vector3 fwdVec = Vector3.up;
            Vector3 upVec = -playerToMe.normalized;
            Vector3.OrthoNormalize(ref upVec, ref fwdVec);
            Quaternion newRot = Quaternion.LookRotation(fwdVec, upVec);
            _stateMachine.rbody.MoveRotation(newRot);

            // set field values
            _destination = playerToDest;
            _currentRadius = playerToMe.magnitude;
            _finalRadius = playerToDest.magnitude;
            _cumulativeAngle = 0.0f;

            // make sure radii cannot exceed max. (Can happen in some edge cases)
            _currentRadius = Mathf.Clamp(_currentRadius, 0f, _stateMachine.player.maxTetherLength);
            _finalRadius =  Mathf.Clamp(_finalRadius, 0f, _stateMachine.player.maxTetherLength);

            Debug.Log($"current radius {Vector3.Distance(_stateMachine.transform.position, _stateMachine.player.transform.position)}");

            // Todo prove this covers all edge cases
            // Gets total angle between position and destination. 
            _totalAngleToDest = Vector3.Angle(playerToMe, playerToDest);
            if (_tf.position.y < _destination.y) {
                // Vector3.Angle gives us angle of shortest path, 
                // but if position is lower than dest, we need longer path
                _totalAngleToDest = 360.0f - _totalAngleToDest;
            }
        }

        public override void Exit()
        {
            base.Exit();
            // move from current rotation to upright rotation
            Vector3 newRotEulers = new Vector3(0.0f, _tf.rotation.y, 0.0f);
            _tf.rotation = Quaternion.identity;
            if (IsGrounded()) {
                _stateMachine.LandedEvent?.Invoke();
            }
            Debug.Log($"current radius {Vector3.Distance(_stateMachine.transform.position, _stateMachine.player.transform.position)}");

        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // rotate around the player and adjust radius
            float angleDelta = _stateMachine.flingSpeed * Time.fixedDeltaTime;
            _tf.RotateAround(_playerTf.position, _playerTf.right, angleDelta);

            Vector3 playerDirToMe = (_tf.position - _playerTf.position).normalized;
            _tf.position = _playerTf.position + playerDirToMe * _currentRadius;

            // update cumulative angle and check if destination is reached
            _cumulativeAngle += angleDelta;
            if (_cumulativeAngle >= _totalAngleToDest) {
                _stateMachine.ChangeState(_stateMachine.idleState);
            }
        }

        protected override void OnPlayerFlingInterrupted()
        {
            base.OnPlayerFlingInterrupted();
            _stateMachine.ChangeState(_stateMachine.idleState); // idle will also allow falling
        }

        protected override void OnCollisionEnter(Collision coll) {
            // TODO this should probably have more conditional logic.
            // Handles hitting wall or floor. If IsGrounded fails, idle goes to falling
            _stateMachine.ChangeState(_stateMachine.idleState); 
        }

        protected override bool CanBeFlung() => false;    
    }
}