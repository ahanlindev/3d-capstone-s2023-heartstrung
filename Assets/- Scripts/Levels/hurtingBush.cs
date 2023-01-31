using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtingBush : MonoBehaviour
{

    private bool alive = true;
    public float radius = 5.0F;
    public float power = 10.0F;
    private HashSet<Transform> collidedBodies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (alive)
        {
            Debug.Log(collision.transform.name);


            if (collision.gameObject.tag == "Player")
            {
                //Transform collidedObj = collision.collider.transform;
                //collidedBodies.Add(collidedObj);
               
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, transform.position, radius, 3.0F);
            }
        }

    }
}
