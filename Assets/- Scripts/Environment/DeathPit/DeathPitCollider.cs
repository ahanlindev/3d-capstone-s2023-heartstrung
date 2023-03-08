using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>The collider of a DeathPit.</summary>
public class DeathPitCollider : MonoBehaviour
{
    [Tooltip(".")]
    [SerializeField] public DeathPit Pit;

    // works as a barrier that is hit
    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Kitty" || col.gameObject.tag == "Player"  || col.gameObject.tag == "Heart") {
            Pit.Death();
        }
    }

    // works as a volume that's safe to exist within
    void OnTriggerExit(Collider col) {
        if(col.gameObject.tag == "Kitty" || col.gameObject.tag == "Player"  || col.gameObject.tag == "Heart") {
            Pit.Death();
        }
    }
}
