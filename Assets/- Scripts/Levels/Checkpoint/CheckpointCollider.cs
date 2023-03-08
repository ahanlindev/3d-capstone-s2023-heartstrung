using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollider : MonoBehaviour
{
    public Checkpoint checkpoint;

    void OnTriggerEnter(Collider other) {
        if(checkpoint.activated) {
            return;
        }
        if(other.gameObject.tag == "Kitty" || other.gameObject.tag == "Player"  || other.gameObject.tag == "Heart") {
            checkpoint.activate();
        }
    }
}
