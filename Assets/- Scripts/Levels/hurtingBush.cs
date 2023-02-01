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

    void OnTriggerEnter(Collider other)
    {
        if (alive)
        {

            // assume we have this PSM
            var playerComponent = other.GetComponent<PlayerStateMachine>();
            if (playerComponent != null)
            {
                playerComponent.GetHurt();

                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb)
                    rb.AddExplosionForce(power, transform.position, radius, 3.0F);
                return;
            }

            var heartComponent = other.GetComponent<Heart>();
            if (heartComponent != null)
            {
                //heartComponent.GetHurt(); // TODO implement heart hurt method
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb)
                    rb.AddExplosionForce(power, transform.position, radius, 3.0F);
                return;
            }
        }
    }

    public void bushDie()
    {
        alive = false;
        Debug.Log("bush is now dead");

        //The bush will no longer block the way, may apply change color or disable the whole cube.
        //GetComponent<BoxCollider>().enabled = false;
        gameObject.SetActive(false);
    }
}
