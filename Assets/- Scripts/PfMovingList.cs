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
    private static int index = 0;
    private float localTimer = 0;
   
    private Transform collidedObj;
    private Vector3 lastPosition;

    [Tooltip("Time for the platform waiting at one waypoint.")] 
    [SerializeField] public float hangTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
    
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveTo();
    }


    void moveTo()
    {
        if (localTimer > 0)
        {
            localTimer -= Time.fixedDeltaTime;
        }
        else if (transform.position.x > target[index].position.x - delta
         && transform.position.x < target[index].position.x + delta
         && transform.position.z > target[index].position.z - delta
         && transform.position.z < target[index].position.z + delta)
        {
            index = (index + 1) % target.Length;
            localTimer = hangTime;
        }
        else
        {

            //target[index].position = new Vector3(target[index].position.x,
            //    target[index].position.y, target[index].position.z);
            //Vector3 direction = target[index].position - thisPlatform.position;
            //direction /= direction.magnitude;
            //Vector3 offset = direction * Time.fixedDeltaTime * speed;
            //thisPlatform.Translate(offset);

            
            float duration = 1f / speed;
            //SetEase(Ease.InOutSine)
            transform.DOMove(target[index].position, duration);
            Vector3 offset = transform.position - lastPosition;
            lastPosition = transform.position;
            if (collidedObj != null)
            {
                collidedObj.GetComponent<Rigidbody>().transform.position += offset;
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collidedObj = collision.collider.transform;

    }

    void OnCollisionExit(Collision other)
    {
        collidedObj = null;
    }
}
