using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>The collider of a DeathPit.</summary>
public class DeathPitCollider : MonoBehaviour
{
    [Tooltip(".")]
    [SerializeField] public DeathPit Pit;

    void OnCollisionEnter(Collision col) {
        Debug.Log("DeathPitCollider collided with " + col.gameObject.name);
        if(col.gameObject.tag == "Kitty" || col.gameObject.tag == "Heart") {
            Pit.Death();
        }
    }
}
