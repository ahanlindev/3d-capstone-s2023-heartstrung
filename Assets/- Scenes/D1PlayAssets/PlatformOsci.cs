using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformOsci : MonoBehaviour
{
  

    [SerializeField] float speedFactor = 2f;
    private Transform collidedObj;
    private float period;
    private static float magicMagnitude = 3.2f;
    private float mvFactor;
    private float delta = 0.5f;
    private Transform thisPlatform;
    private float stopWatch = 0f;
    private float localTimer = 0f;
    public float hangTime = 3.0f;
    public Transform wayPointOne;
    public Transform wayPointTwo;
    [SerializeField] public Transform[] target;
    private int index = 0;



    void Start()
    {
 
        period = 1 / speedFactor;
        stopWatch = hangTime;
        thisPlatform = transform.GetChild(0);
        //transform.DOMove(new Vector3(2, 2, 2), 2.0f).SetEase(Ease.InOutSine)
        //    .onComplete();
    }

    void FixedUpdate()
    {
        if (stopWatch > 0)
        {
            stopWatch -= Time.fixedDeltaTime;
        
          
        }else if (thisPlatform.position.x > target[index].position.x - delta
         && thisPlatform.position.x < target[index].position.x + delta
         && thisPlatform.position.z > target[index].position.z - delta
         && thisPlatform.position.z < target[index].position.z + delta)
            
        {
            localTimer = hangTime;
            index = (index + 1) % target.Length;
        }
        else
            {
            localTimer += Time.fixedDeltaTime;
            float cycles = localTimer / period;

            const float tau = Mathf.PI * 2;
            float rawSinWave = Mathf.Sin(cycles * tau);

            mvFactor = (rawSinWave) * magicMagnitude;
            Vector3 mvVector = wayPointTwo.position - wayPointOne.position;
            Vector3 offset = mvVector * mvFactor * Time.fixedDeltaTime;
            thisPlatform.Translate(offset);

            //Debug.Log(offset);

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

    private void OnCollisionExit(Collision other)
    {
        collidedObj = null;
    }
}
