using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class EnemyAI : BTree
{
    public NavMeshAgent _agent;

    public UnityEngine.Transform[] waypoints;

    public Health _health;

    public static float fovRange = 4f;

    public static float speed = 3.5f;

    public static float attackRange = 1f;


    public EnemyClaw _enemyClaw;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _enemyClaw = GetComponentInChildren<EnemyClaw>();
    }

    private void OnEnable()
    {
        _health.ChangeHealthEvent += OnChangeHealth;
    }

    private void OnDisable()
    {
        _health.ChangeHealthEvent -= OnChangeHealth;
    }

    private void OnChangeHealth(float newTotal, float delta)
    {
        if (newTotal  <= 0) {
            //play dead animation
            _agent.isStopped = true;
        }
    }

    protected override Node SetupTree() 
    {
        /*
            first sequence is checking if enemy is dead, if not dead then take damage
            second sequence is checking if player is within the attack radius of enemy, if 
                the player is in range then attack the player
            third sequence is checking if player is within the POV radius of enemy, if 
                the player is in range then approach the player
            last sequence is just basic patrol
        */
        Node root = new Selector(new List<Node>
            {
             new Sequence(new List<Node>
                {
                    new KO(_agent, _health),
                    new TakeHit(_health),
            }),
             new Sequence(new List<Node>
                {
                new CheckPlayerInRange(_agent), 
                new Attack(_agent, _enemyClaw),
            }), 
             new Sequence(new List<Node>
                {new CheckFOVRange(_agent),
                new GoToPlayer(_agent),
            }), 
             new Patrol(_agent, waypoints),
        });

        return root;
    }
}