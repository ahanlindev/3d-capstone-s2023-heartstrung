using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private NavMeshAgent _navAgent;

    private enum State {WANDER, CHASE, DEAD, HURT, ATTACK};
    private State _currentState;
    [SerializeField] private float _respawnTime = 10;
    [SerializeField] private int _enemyHealth = 10;

    //use raycast to detect dodger
    private Vector3 _playerPosition;

    [SerializeField]
    private List<Transform> _waypoints;
    private int _target;
    
    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _currentState = State.WANDER;
        if (_navAgent is null) {
            Debug.LogError("Nav Mesh Agent is NULL");
        }
        if (_waypoints.Count > 1 && !(_waypoints[0] is null)) {
            _navAgent.SetDestination(_waypoints[0].position);
        } else {
            Debug.LogError("Please select 2 waypoints at least");
        }
    }

    private void OnTriggerEnter(Collider other) 
    {   
        //maybe change to random pos on navmesh instead of patrol?
        if (other.CompareTag("Waypoints")) {
            _target++;

            if (_target == _waypoints.Count) {
                _waypoints.Reverse();
                _target = 1;
            }

            if (!(_waypoints[_target] is null)) {
                _navAgent.SetDestination(_waypoints[_target].position);
            } else {
                Debug.LogError("Target waypoint is null");
            }
        }
    }

    private void OnEnable() 
    {
        EnemyEyes eyes = GetComponentInChildren<EnemyEyes>();
        eyes.OnHeartSeenEvent += StartChase;
        eyes.OnKittySeenEvent += StartChase;
        eyes.OnHeartGoneEvent += StopChase;
        eyes.OnKittyGoneEvent += StopChase;
    }

    public void OnDisable()
    {
        EnemyEyes eyes = GetComponentInChildren<EnemyEyes>();
        eyes.OnHeartSeenEvent -= StartChase;
        eyes.OnKittySeenEvent -= StartChase;
        eyes.OnHeartGoneEvent -= StopChase;
        eyes.OnKittyGoneEvent -= StopChase;
    }

    private void OnDead() 
    {
        _currentState = State.DEAD;
        StartCoroutine(DeathTimer());
    }

    private IEnumerator DeathTimer() 
    {
        yield return new WaitForSeconds(_respawnTime);
        _currentState = State.WANDER;
    }

    private void OnHurt() 
    {
        if (_enemyHealth < 1) {
            OnDead();
        } else {
            //play knockback animation and knock enemy back

            //get current position, maybe normalize?
        }
    }

    private void ExecuteAttack() 
    {
        //calculate the distance between enemy and dodge, if within range, execute attack
        //on hit, emit signal to dodge to notify that he is hurt
    }

    private void StartChase(Vector3 playerPos) 
    {
        /* if dodger or kitty is within the vision of enemy, then change state to chase and
        start moving towards dodge */
        _currentState = State.CHASE;
        _playerPosition = playerPos;
    }

    private void StopChase() {
        _currentState = State.WANDER;
    }

    void Update() 
    {
        switch (_currentState) {
            case State.WANDER: 
                break;
            case State.CHASE:
                _navAgent.SetDestination(_playerPosition);
                break;
            case State.DEAD:
                //pause the enemy for x amount of time
                break;
            case State.ATTACK:
                //play the attack animation and deal dmg to dodge & kitty
                break;
            case State.HURT:
                //play the knockback animation and take dmg and pause for a little
                break;
            default:
                break;
        }   
    }
}
