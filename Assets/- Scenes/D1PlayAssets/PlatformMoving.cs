using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame


    //public float hangningtime = 0f;
    private Vector3 startingPos;
 

  
    [SerializeField] [Range(0, 5)] float mvFactor;
    [SerializeField] float period = 2f;
    Transform collidedObj;
    private static float magicMagnitude = 1.6f;
    public Transform wayPointOne;
    public Transform wayPointTwo;


    void Start()
    {
       
        startingPos = wayPointOne.position;
       
    }

    void FixedUpdate()
    {
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        mvFactor = (rawSinWave) * magicMagnitude;
        Vector3 mvVector = wayPointTwo.position - wayPointOne.position;
        Vector3 offset = mvVector * mvFactor * Time.fixedDeltaTime;
        transform.position += offset;
        Debug.Log(offset);

        if (collidedObj != null)
        {
            collidedObj.GetComponent<Rigidbody>().transform.position += offset;
        }
       
    }


    private void OnCollisionEnter(Collision collision)
    {
        collidedObj = collision.collider.transform;

    }
}
