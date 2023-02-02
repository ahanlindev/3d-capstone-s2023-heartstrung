using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class OldHeart : MonoBehaviour
{
    private enum State { IDLE, FLUNG, FALLING, HURT, DEAD };

    // inspector fields
    [Tooltip("PlayerController instance for the player attached to this object")]
    [SerializeField] private OldPlayerController player;
    
    [Tooltip("Linear speed along the arc that the heart will fly through the air in units per second")]
    [SerializeField] private float flingSpeed = 10;

    // private fields
    // rigidbody of the heart
    private Rigidbody rbody;
    
    // collider of the heart
    private Collider coll;
    
    // current state of the heart. 
    [SerializeField] private State currentState; // TODO deserialize after debug

    // Used when flinging. Fling destination as a point in space.
    private Vector3 currentDestination;
    
    // Used when flinging. Angle in degrees from heart's start point to destination, using player as the pivot.
    private float totalFlingAngle;

    // Used when flinging. Radius in units between player and heart when the fling begins
    private float initialFlingRadius;

    // Emitted events
    public event Action LandedEvent;

    // Setup/Teardown
    private void Awake() {
        if (!player) { Debug.LogError("Heart script has no Player set!"); }

        rbody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    private void OnEnable() {
        OldPlayerController.flingEvent += OnPlayerFling;
        BattleManager.kittyTakeDmgEvent += OnTakeDamage;
    }

    private void OnDisable() {
        OldPlayerController.flingEvent -= OnPlayerFling;
        BattleManager.kittyTakeDmgEvent -= OnTakeDamage;

    }

    // Accessors

    public bool CanBeFlung() => CanChangeState(State.FLUNG);

    // Per-tick updates
    
    private void FixedUpdate() {
        if (currentState == State.FLUNG) {
            DoFlingTick();
        }    
    }

    // Event Handlers

    // TODO this is for the jam. Shakes the character model without shaking the collider
    void OnTakeDamage(int dmg) {
        Debug.Log("Ow"); 
        transform.DOShakeRotation(0.3f)
        .SetDelay(0.5f)
        .OnStart(() => rbody.constraints = 0)
        .OnComplete(()=> rbody.constraints = RigidbodyConstraints.FreezeRotation);
    }

    /// <summary>Subscribes to the player's fling event. Will fling this object around the player</summary>
    void OnPlayerFling(float power) {
        if (TryChangeState(State.FLUNG)) {
            IgnorePlayerCollisions(true);
            currentDestination = CalculateDestination(power);
            totalFlingAngle = CalculateAngleToDestination(currentDestination);
            initialFlingRadius = Vector3.Distance(transform.position, player.transform.position);
        }
    }
    
    /// <summary>
    /// Moves the object around the player towards the destination over the course of a single physics tick
    ///</summary>
    private void DoFlingTick() { 
        if (transform.position != currentDestination) {

            // move orthogonal to player and their left vector. Will result in an arc over their head
            Vector3 vecFromPlayer = transform.position - player.transform.position;
            Vector3 moveDir = Vector3.Cross(vecFromPlayer, -player.transform.right).normalized;
            Vector3 movement = moveDir * flingSpeed * Time.fixedDeltaTime;

            // apply linear movement
            Vector3 newPos = transform.position + movement;

            // pull new position to within proper radius
            // float remainingAngle = CalculateAngleToDestination(currentDestination);
            // float radiusLerpRatio = (totalFlingAngle - remainingAngle) / totalFlingAngle;
            // float destRadius = Vector3.Distance(player.transform.position, currentDestination);
            //float newRadius = Mathf.Lerp(initialFlingRadius, destRadius, radiusLerpRatio);
            
            Vector3 newVecFromPlayer = newPos - player.transform.position;
            float newRadius = initialFlingRadius; // TODO this is a hack for the jam. Ignores power.
            newPos = player.transform.position + (newVecFromPlayer.normalized) * newRadius;
            rbody.useGravity = false; // TODO does this help
            rbody.MovePosition(newPos);
        }
        else {
            TryFinishFling();
        }
    }

    /// <summary>
    /// Attempts to go from FLUNG to another state. 
    /// Usually idle or falling. Emits LandedEvent if successful.
    /// </summary>
    private bool TryFinishFling() {
        rbody.useGravity = true; // TODO does this help
        IgnorePlayerCollisions(false);
        if (IsGrounded()) {
            // Heart has landed. Go into idle mode and emit event
            TryChangeState(State.IDLE);
            LandedEvent?.Invoke();
            return true;
        } else {
            // Heart has hit a wall. Can't finish yet. Start falling.
            TryChangeState(State.FALLING);
            return false;
        }
    }

    private void OnCollisionEnter(Collision other) {
        // hit wall or floor
        if (currentState == State.FLUNG || currentState == State.FALLING) {
            TryFinishFling();
        }
    }


    // State management

    /// <summary>Check if the current state has a valid transition to the desired new state.</summary>
    /// <param name="newState">the desired new state.</param>
    /// <returns>true if transition to newState will work, false otherwise.</returns>
    private bool CanChangeState(State newState) {
        // result may change depending on combination of current and new state
        bool result = false;
        
        switch (currentState) {
            case State.IDLE: 
                // heart can go from idle to any other state 
                result = true;
                break;
            case State.FLUNG: 
                // heart should not get hurt while mid-fling
                result = true;
                break;
            case State.FALLING: 
                // heart should not be flung while falling
                result = (newState != State.FLUNG);
                break;
            case State.HURT: 
                // heart can go from hurt to any other state
                result = true;
                break;
            case State.DEAD: 
                // if heart is dead, cannot change state
                result = false;
                break;
            default:
                Debug.LogError("Unhandled Heart State encountered!");
                break;
        }

        return result;
    } 

    /// <summary>Attempts to change the player's current state to the parameter.</summary>
    /// <param name="newState">the state attempting to replace the current state.</param>
    /// <returns>true if change was successful, false otherwise.</returns>
    private bool TryChangeState(State newState) {
        if (!CanChangeState(newState)) { return false; }

        currentState = newState;
        return true;
    }

    // Helper methods 

    /// <summary>Toggles whether the heart is able to be moved in various axes.</summary>
    /// <param name="freezeX">if true, the rigidbody's x position will not be constrained.</param>
    void ToggleFreezePosition(bool freezeX = false, bool freezeY = false, bool freezeZ = true) {
        // constraints are a bunch of bit flags, so use bitwise OR to set appropriately

        // rotation should always be constrained
        RigidbodyConstraints rotConstraints = RigidbodyConstraints.FreezeRotation;
        
        // position is conditionally restrained
        var xConstraint = (freezeX) ? RigidbodyConstraints.FreezePositionX : 0;
        var yConstraint = (freezeY) ? RigidbodyConstraints.FreezePositionY : 0;
        var zConstraint = (freezeZ) ? RigidbodyConstraints.FreezePositionZ : 0;

        // set rigidbody constraints to combination of pos and rot
        rbody.constraints = rotConstraints | xConstraint | yConstraint | zConstraint;
    }

    Vector3 CalculateDestination(float power) {
        Vector3 playerPos = player.transform.position;
        Vector3 pos = transform.position;

        // find vector from heart to player
        var vecToPlayer = playerPos - pos;
        
        // destination is radius between player and heart, in the forward dir of the player, scaled by power.
        var vecFromPlayer = player.transform.forward * vecToPlayer.magnitude * power;
        return Vector3.zero;
    }

    /// <summary>
    /// TODO currently broken
    /// Returns the angle in degrees between the heart and 
    /// its destination, with the player's position as the origin
    /// </summary>
    /// <param name="destination">The desired destination position</param>
    float CalculateAngleToDestination(Vector3 destination) {
        // Vector3 playerPos = player.transform.position;
        // Vector3 pos = transform.position;

        // // find vectors from player to heart, and from player to destination
        // var playerVecToHeart = pos - playerPos;
        // var playerVecToDest = destination - playerPos;

        // return Vector3.Angle(playerVecToHeart, playerVecToDest);
        return 0;
    }

    /// <summary>Makes all non-trigger colliders of the heart and the player either ignore each other or recognize each other</summary>
    /// <param name="ignoreCollision">if true, the player and heart will ignore each other. Otherwise they will not.</param>
    private void IgnorePlayerCollisions(bool ignoreCollision) {
        foreach (Collider mycol in GetComponentsInChildren<Collider>()) {
            foreach (Collider plycol in player.GetComponentsInChildren<Collider>()) {
                if (!mycol.isTrigger && !plycol.isTrigger) {
                    Physics.IgnoreCollision(mycol, plycol, ignoreCollision);
                }
            }
        }
    }

    /// <summary>Checks if heart is grounded using a raycast.</summary>
    /// <returns>True if the heart is grounded, otherwise false.</returns>
    // TODO currently can fail on edges of platform. Add coyote time.
    private bool IsGrounded() {
        // send raycast straight downward. If it hits nothing, the player must be airborne.
        float distToGround = coll.bounds.extents.y;
        bool castHit = Physics.BoxCast(transform.position, new Vector3(1,.4f,1), -transform.up, Quaternion.identity, 1.0f);
        // for (int x = -1; x <= 1; x++) {
        //     for (int z = -1; z <= 1; z++) {
        //         float deltaX = 0.25f * x;
        //         float deltaZ = 0.25f * z;
                
        //         Vector3 origin = transform.position;
        //         origin.x += deltaX;
        //         origin.z += deltaZ;
        //         castHit |= Physics.Raycast(origin, -transform.up, distToGround + 0.1f);
        //     }
        // }
        return castHit;
    }
}