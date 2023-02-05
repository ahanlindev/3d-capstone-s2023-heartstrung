using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using BehaviorTree;

public class EnemyAI : BTree
{
    public NavMeshAgent _agent;

    public UnityEngine.Transform[] waypoints;

    public Health _health;

    public static float fovRange = 6f;

    public static float speed = 3.5f;

    public static float attackRange = 1f;

    public bool _dead = false;

    public EnemyClaw _enemyClaw;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _enemyClaw = GetComponent<EnemyClaw>();
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
            _dead = true;
        }
    }

    protected override Node SetupTree() 
    {
        Node root = new Selector(new List<Node>
            {
             new Sequence(new List<Node>
                {
                    new TakeHit(_health, _dead),
                    new KO(_agent),
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