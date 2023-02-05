using UnityEngine;
using DG.Tweening;

namespace Heart
{
    /// <summary>State where the heart is flying through the air towards a destination.</summary>
    public class Flung : State
    {
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

            _playerTf = _sm.player.transform;
            _tf = _sm.transform;

            // temporarily ignore player collision and gravity
            IgnorePlayerCollisions(true);
            _sm.rbody.useGravity = false;

            // get player->me and player->dest vectors
            Vector3 playerToMe = _tf.position - _playerTf.position;
            Vector3 playerToDest = new Vector3(playerToMe.x, 0f, playerToMe.z).normalized;
            playerToDest *= -playerToMe.magnitude * _sm.flingPower;

            // Lin-alg wizardry to face upward, with stem facing player, so rotation looks nicer
            RotateStemTowardsPlayer(faceUp: true);

            // set field values
            _destination = playerToDest;
            _currentRadius = playerToMe.magnitude;
            _finalRadius = playerToDest.magnitude;
            _cumulativeAngle = 0.0f;

            // make sure radii cannot exceed max. (Can happen in some edge cases)
            _currentRadius = Mathf.Clamp(_currentRadius, 0f, _sm.player.maxTetherLength);
            _finalRadius = Mathf.Clamp(_finalRadius, 0f, _sm.player.maxTetherLength);

            // Todo prove this covers all edge cases
            // Gets total angle between position and destination. 
            _totalAngleToDest = Vector3.Angle(playerToMe, playerToDest);
            if (_tf.position.y < _destination.y)
            {
                // Vector3.Angle gives us angle of shortest path, 
                // but if position is lower than dest, we need longer path
                _totalAngleToDest = 360.0f - _totalAngleToDest;
            }
        }

        public override void Exit()
        {
            base.Exit();
            _sm.rbody.useGravity = true;

            // move from current rotation to upright rotation
            Vector3 newRotEulers = new Vector3(0.0f, _tf.rotation.y, 0.0f);
            _tf.rotation = Quaternion.identity;
            if (IsGrounded())
            {
                _sm.LandedEvent?.Invoke();
            }

            // allow player collisions again
            IgnorePlayerCollisions(false);
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();

            // rotate around the player and adjust radius
            float angleDelta = _sm.flingSpeed * Time.fixedDeltaTime;

            Vector3 playerDirToMe = (_tf.position - _playerTf.position).normalized;

            playerDirToMe = Quaternion.AngleAxis(angleDelta, _playerTf.right) * playerDirToMe;
            _tf.position = _playerTf.position + playerDirToMe * _currentRadius;

            RotateStemTowardsPlayer();

            // update cumulative angle and check if destination is reached
            _cumulativeAngle += angleDelta;
            if (_cumulativeAngle >= _totalAngleToDest)
            {
                _sm.ChangeState(_sm.idleState);
            }
        }

        protected override void OnPlayerFlingInterrupted()
        {
            base.OnPlayerFlingInterrupted();
            _sm.ChangeState(_sm.idleState); // idle will also allow falling
        }

        protected override void OnCollisionEnter(Collision coll)
        {
            // TODO this should probably have more conditional logic.
            // Handles hitting wall or floor. If IsGrounded fails, idle goes to falling
            _sm.ChangeState(_sm.idleState);
        }

        protected override bool StateIsFlingable() => false;

        // Helper methods

        /// <summary>Makes all non-trigger colliders of the heart and the player either ignore each other or recognize each other</summary>
        /// <param name="ignoreCollision">if true, the player and heart will ignore each other. Otherwise they will not.</param>
        private void IgnorePlayerCollisions(bool ignoreCollision)
        {
            foreach (Collider mycol in _tf.GetComponentsInChildren<Collider>())
            {
                foreach (Collider plycol in _playerTf.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(mycol, plycol, ignoreCollision);
                }
            }
        }

        /// <summary>Manipulates the rotation of the heart so that it faces stem towards the player</summary>
        /// <param name="faceUp">if true, the forward vector of the heart will face directly upward. Used when starting the fling.</param>
        private void RotateStemTowardsPlayer(bool faceUp = false)
        {
            // get player->me vector
            Vector3 playerToMe = _tf.position - _playerTf.position;

            // Lin-alg wizardry to face upward, with stem facing player, so rotation looks nicer
            Vector3 fwdVec = (faceUp) ? Vector3.up : _tf.forward;
            Vector3 upVec = -playerToMe.normalized;
            Vector3.OrthoNormalize(ref upVec, ref fwdVec);
            Quaternion newRot = Quaternion.LookRotation(fwdVec, upVec);
            _tf.rotation = newRot;
        }
    }
}