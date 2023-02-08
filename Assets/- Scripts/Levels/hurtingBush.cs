using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtingBush : MonoBehaviour
{

    private bool alive = true;
    public float radius = 5.0F;
    public float damage = 10.0F;
    public float power = 5.0F;
    private HashSet<Transform> collidedBodies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alive)
        {

            var healthComponent = other.gameObject.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.ChangeHealth(-damage);

                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    Vector3 dir = other.transform.position - transform.position;
                    rb.AddForce(dir.normalized * power, ForceMode.VelocityChange);
                }
     
            }


            //var heartComponent = other.GetComponent<Heart>();
            //if (heartComponent != null)
            //{
            //    heartComponent.GetHurt();
            //    Rigidbody rb = other.GetComponent<Rigidbody>();
            //    if (rb)
            //        rb.AddExplosionForce(power, transform.position, radius, 3.0F);
            //    return;
            //}

        }
    }

    public void BushDie()
    {
        alive = false;

        //The bush will no longer block the way, may apply change color or disable the whole cube.
        Destroy(transform.parent.gameObject);
    }
}
