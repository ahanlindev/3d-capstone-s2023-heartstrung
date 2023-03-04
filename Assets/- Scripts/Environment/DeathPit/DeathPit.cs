using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Teleports Kitty and Dodger when collided with.</summary>
public class DeathPit : MonoBehaviour
{
    public float damageTaken = 10f;

    public void Death() {
        Debug.Log("Fell into a DeathPit!");
        PlayerStateMachine player = FindObjectOfType<PlayerStateMachine>();
        HeartStateMachine heart = player.heart;
        heart?.health.ChangeHealth(-damageTaken);

        // move Kitty and Dodger to safety if they're alive
        if (heart?.health.CurrentHealth > 0) {
            CheckpointManager cm = FindObjectOfType<CheckpointManager>();
            cm.ResetToCheckpoint();
        }
    }
}
