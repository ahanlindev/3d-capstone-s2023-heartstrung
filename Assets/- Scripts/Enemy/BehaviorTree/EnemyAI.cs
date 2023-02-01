using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using BehaviorTree;

public class EnemyAI : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 3.5f;

    protected override Node SetupTree() 
    {
        Node root = new Patrol(transform, waypoints);
        return root;
    }

}