using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
// Attach this script to the "Player Pair" gameObject.
// Manages the respawn location of the player if they die.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    public GameObject Kitty;
    public GameObject Dodger;
    public Vector3 KittyRespawnLocation;
    public Vector3 DodgerRespawnLocation;
    public bool tutorialPlayer;

    // Start is called before the first frame update
    void Start() {
        // If this is a player pair, get the transforms of
        // Kitty and Dodger as initial checkpoints
        if(!tutorialPlayer) {
            DodgerRespawnLocation = new Vector3(
                Dodger.transform.position.x,
                Dodger.transform.position.y,
                Dodger.transform.position.z);
            KittyRespawnLocation = new Vector3(
                Kitty.transform.position.x,
                Kitty.transform.position.y,
                Kitty.transform.position.z);
        } else {
            // use the transform of the gameobject this script is attached to
            Debug.LogWarning("Dodger not located! Are we in Tutorial 1?");
            KittyRespawnLocation = new Vector3(
                this.gameObject.transform.position.x,
                this.gameObject.transform.position.y,
                this.gameObject.transform.position.z);
        }
    }

    /// <summary>
    /// Called when a checkpoint is reached.
    /// Updates the respawn locations of Kitty and Dodger.
    /// </summary>
    public void UpdateCheckpoint(Checkpoint c) {
        this.KittyRespawnLocation = c.KittyRespawnLocation.transform.position;
        this.DodgerRespawnLocation = c.DodgerRespawnLocation.transform.position;
    }

    /// <summary>
    /// Reset Kitty and Dodger to the previous checkpoint.
    /// </summary>
    public void ResetToCheckpoint() {
        Debug.Log("Resetting to last checkpoint...");

        if(!tutorialPlayer) {
            Kitty.gameObject.transform.position = KittyRespawnLocation;
            Dodger.gameObject.transform.position = DodgerRespawnLocation;
            // Debug.Log("Resetting velocity");
            Kitty.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            Dodger.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        } else {
            // This is the tutorial player, so do it a bit differently
            this.gameObject.transform.position = KittyRespawnLocation;
        }
    }
}
