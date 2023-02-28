using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PfMovingList : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3.0f; //

    [Tooltip("Add/delete empty objects to the target list to set new waypoints.")]
    [SerializeField] public Transform[] target;  //
    private float delta = 0.05f; // 
    private int index = 0;
    private float localTimer = 0;
   
    private Vector3 lastPosition;
    private int tgtLength = 0;

    private HashSet<Rigidbody> collidedBodies;

    [Tooltip("Time for the platform waiting at one waypoint.")] 
    [SerializeField] public float hangTime = 3.0f;

    [Tooltip("If it stop after reach the final wp of the list")]
    [SerializeField] public bool oneWay;
    
    private bool _moved;


    // Update is called once per frame
    void FixedUpdate()
    {
        
        moveTo();
    }

    private void OnEnable() {
        lastPosition = transform.position;
        if (collidedBodies == null) collidedBodies = new HashSet<Rigidbody>();
        tgtLength = target.Length;

        transform.DOMove(target[index].position, 1 / speed);
    }

    private void OnDisable() {
        collidedBodies = null;    
    }

    void moveTo()
    {
       

        if (localTimer > 0)
        {
            localTimer -= Time.fixedDeltaTime;
        }
        
        else if ((transform.position - target[index].position).magnitude < delta)
        {
            if (!_moved)
            {
                index = (index + 1) % tgtLength;
                //if (speed == 1.5f)
                //{
                //    Debug.Log(index);
                //}
                transform.DOMove(target[index].position, 1 / speed);
                localTimer = hangTime + 1 / speed;
                if (index == tgtLength - 1 && oneWay)
                {
                    _moved = true;
                }
            }
        }
 
            //target[index].position = new Vector3(target[index].position.x,
            //    target[index].position.y, target[index].position.z);
            //Vector3 direction = target[index].position - thisPlatform.position;
            //direction /= direction.magnitude;
            //Vector3 offset = direction * Time.fixedDeltaTime * speed;
            //thisPlatform.Translate(offset);

            
        
            //SetEase(Ease.InOutSine)
            Vector3 offset = transform.position - lastPosition;
            lastPosition = transform.position;
            foreach(Rigidbody attached in collidedBodies)
            {
                attached.MovePosition(attached.transform.position + offset);
            }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var temp = collision.collider.attachedRigidbody;
        if (temp != null) collidedBodies.Add(temp);
    }

    private void OnCollisionExit(Collision collision)
    {
        var temp = collision.collider.attachedRigidbody;
        if (temp != null) collidedBodies.Remove(temp);
    }
}
