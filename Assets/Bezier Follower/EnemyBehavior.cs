using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Bezier;

public class EnemyBehavior : MonoBehaviour{
public Path pathToFollow;

//PATH INFO
public int currentWayPointID = 0;

//speeds
public float speed = 2;
public float reachDistance = 0.4f;
public float rotationSpeed = 5f;

float distance; //DISTANCE TO NEXT PATH POINT

public bool useBezier = false;

//STATE MACHINES
public enum EnemyStates
{
    ON_PATH,        
    IDLE
}
public EnemyStates enemyState;

public int enemyID;

void Update()
{
    switch (enemyState)
    {
        case EnemyStates.ON_PATH:
            MoveOnThePath(pathToFollow);
            break;            
        case EnemyStates.IDLE:

            break;
    }
}

void MoveToFormation()
{
    //transform.position = Vector3.MoveTowards(transform.position, formation.gridList[enemyID], speed * Time.deltaTime);
    //if(Vector3.Distance(transform.position, formation.gridList[enemyID])<= 0.001f)
    {
        //transform.SetParent(formation.gameObject.transform);
        transform.eulerAngles = Vector3.zero;
        enemyState = EnemyStates.IDLE;
    }
}

void MoveOnThePath(Path path)
{
    if (useBezier)
    {
        //MOVING THE ENEMY
        distance = Vector3.Distance(path.bezierObjList[currentWayPointID], transform.position);
        transform.position = Vector3.MoveTowards(transform.position, path.bezierObjList[currentWayPointID], speed * Time.deltaTime);
        //ROTATION OF YOUR ENEMY
        var direction = path.bezierObjList[currentWayPointID] - transform.position;

        if (direction != Vector3.zero)
        {
            direction.z = 0;
            direction = direction.normalized;
            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
    else
    {
        distance = Vector3.Distance(path.pathObjList[currentWayPointID].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, path.pathObjList[currentWayPointID].position, speed * Time.deltaTime);

        //ROTATION OF ENEMY
        var direction = path.pathObjList[currentWayPointID].position - transform.position;

        if (direction != Vector3.zero)
        {
            direction.y = 0;
            direction = direction.normalized;
            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }        
}}