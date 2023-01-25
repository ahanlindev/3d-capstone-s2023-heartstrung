using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private NavMeshAgent _navAgent;
    public float speed = 2; 
    public float delta = 0.2f; 
    private static int i = 0;

    private enum State {WANDER, CHASE, DEAD, HURT, ATTACK};
    private State _currentState;
    [SerializeField] private float _respawnTime = 10;
    [SerializeField] private float _attackTime = 3;

    [SerializeField] private int _enemyHealth = 10;

    //use raycast to detect dodger
    private Vector3 _playerPosition;

    [SerializeField]
    private List<Transform> _waypoints;
    private int _target;

    private EnemyClaw _enemyClaw;
    
    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _enemyClaw = GetComponentInChildren<EnemyClaw>();
        _currentState = State.WANDER;
        if (_navAgent is null) {
            Debug.LogError("Nav Mesh Agent is NULL");
        }

        _navAgent.updateRotation = true;
    }

    private void moveTo()
    {
        _waypoints[i].position = new Vector3(_waypoints[i].position.x, transform.position.y, _waypoints[i].position.z);
        
        transform.LookAt(_waypoints[i]);
        _navAgent.destination = _waypoints[i].position;

        if (_navAgent.transform.position.x > _waypoints[i].position.x - delta
            && _navAgent.transform.position.x < _waypoints[i].position.x + delta
            && _navAgent.transform.position.z > _waypoints[i].position.z - delta
            && _navAgent.transform.position.z < _waypoints[i].position.z + delta)
            i = (i + 1) % _waypoints.Count;
    }

    private void OnEnable() 
    {
        EnemyEyes eyes = GetComponentInChildren<EnemyEyes>();
        eyes.OnPlayerSeenEvent += StartChase;
        eyes.OnHeartSeenEvent += StartChase;
        eyes.OnPlayerGoneEvent += StopChase;
        eyes.OnHeartGoneEvent += StopChase;
    }

    public void OnDisable()
    {
        EnemyEyes eyes = GetComponentInChildren<EnemyEyes>();
        eyes.OnPlayerSeenEvent -= StartChase;
        eyes.OnHeartSeenEvent -= StartChase;
        eyes.OnPlayerGoneEvent -= StopChase;
        eyes.OnHeartGoneEvent -= StopChase;
    }

    public void OnDead() 
    {
        _currentState = State.DEAD;
        StartCoroutine(DeathTimer());
    }

    private IEnumerator DeathTimer() 
    {
        yield return new WaitForSeconds(_respawnTime);
        _currentState = State.WANDER;
    }

    // private void OnHurt() 
    // {
    //     //take dmg

    //     //condition check
    //     if (_enemyHealth < 1) {
    //         OnDead();
    //     } else {
    //         //play knockback animation and knock enemy back

    //         //get current position, maybe normalize?
    //     }
    // }

    private void ExecuteAttack() 
    {
        //deal dmg to kitty
        _enemyClaw.Claw(1.0f);
        StartCoroutine(AttackTimer());
    }

    private IEnumerator AttackTimer() 
    {
        
        yield return new WaitForSeconds(_attackTime);
        _currentState = State.CHASE;
    }

    private void StartChase(Vector3 playerPos) 
    {
        /* if dodger or kitty is within the vision of enemy, then change state to chase and
        start moving towards dodge */
        _currentState = State.CHASE;
        _playerPosition = playerPos;
    }

    private void StopChase() 
    {
        _currentState = State.WANDER;
    }

    void Update() 
    {
        switch (_currentState) {
            case State.WANDER: 
                _navAgent.isStopped = false;
                moveTo();
                break;
            case State.CHASE:
                _navAgent.isStopped = false;
                _navAgent.transform.LookAt(new Vector3(_playerPosition.x, transform.position.y, _playerPosition.z));
                _navAgent.destination = _playerPosition;
                if (_navAgent.transform.position.x > _playerPosition.x - delta
                    && _navAgent.transform.position.x < _playerPosition.x + delta
                    && _navAgent.transform.position.z > _playerPosition.z - delta
                    && _navAgent.transform.position.z < _waypoints[i].position.z + delta) {
                        _currentState = State.ATTACK;
                        ExecuteAttack();
                    }
                break;
            case State.DEAD:
                //pause the enemy for x amount of time
                _navAgent.isStopped = true;
                break;
            case State.ATTACK:
                //play the attack animation and deal dmg to dodge & kitty
                _navAgent.isStopped = true;
                break;
            // case State.HURT:
            //     play the knockback animation and take dmg and pause for a little
            //     break;
            default:
                break;
        }   
    }
}
