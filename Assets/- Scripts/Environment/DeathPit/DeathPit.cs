using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Teleports Kitty and Dodger when collided with.</summary>
public class DeathPit : MonoBehaviour
{
    public GameObject KittyRespawnPoint;
    public GameObject DodgerRespawnPoint;
    public float damageTaken = 10f;

    public void Death() {
        Debug.Log("OWIE!");
        PlayerStateMachine player = FindObjectsOfType<PlayerStateMachine>()[0];
        HeartStateMachine heart = player.heart;
        heart.health.ChangeHealth(-damageTaken);

        // move Kitty and Dodger to safety
        player.gameObject.transform.position = new Vector3(
            KittyRespawnPoint.transform.position.x,
            KittyRespawnPoint.transform.position.y,
            KittyRespawnPoint.transform.position.z);
        heart.gameObject.transform.position = new Vector3(
            DodgerRespawnPoint.transform.position.x,
            DodgerRespawnPoint.transform.position.y,
            DodgerRespawnPoint.transform.position.z);
    }
}
