using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using BehaviorTree;

public class EnemyAI : Tree
{
    public NavMeshAgent _agent;
    public UnityEngine.Transform[] waypoints;

    public static float fovRange = 6f;

    public static float speed = 3.5f;

    public static float attackRange = 1f;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
            {
            new Sequence(new List<Node>
                {new CheckFOVRange(_agent),
                new GoToPlayer(_agent),
            }), 
            new Patrol(_agent, waypoints),
        });
        return root;
    }


    // protected override Node SetupTree() 
    // {
    //     Node root = new Selector(new List<Node>
    //         {new Sequence(new List<Node>
    //             {new CheckPlayerInRange(transform),
    //             new Attack(transform),
    //         }), 
    //         new Sequence(new List<Node>
    //             {new CheckFOVRange(transform),
    //             new GoToPlayer(transform),
    //         }), 
    //         new Patrol(transform, waypoints),
    //     });

    //     return root;
    // }
}