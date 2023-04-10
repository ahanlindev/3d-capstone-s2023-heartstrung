using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete] public class HurtingBush : MonoBehaviour
{

    private bool alive = true;
    public float radius = 5.0F;
    public float damage = 10.0F;
    public float power = 5.0F;

    private HashSet<Transform> collidedBodies;

    // Start is called before the first frame update
    void Start()
    {
        var health = GetComponent<Health>();
        if (health) {
            health.ChangeHealthEvent += BushDie;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
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
    }

    /// <summary>Destroys the bush. Called whenever the health of the bush is changed.</summary>
    /// <param name="_">Unused. Needed to match signature of subscribed event.</param>
    /// <param name="__">Unused. Needed to match signature of subscribed event.</param>
    public void BushDie(float _, float __)
    {
        //The bush will no longer block the way, may apply change color or disable the whole cube.
        Destroy(transform.parent.gameObject);
    }
}
