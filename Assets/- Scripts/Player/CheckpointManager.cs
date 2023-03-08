using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
// Attach this script to the "Player Pair" gameObject.
// Manages the respawn location of the player if they die.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private GameObject _kitty;
    [SerializeField] private GameObject _dodger;
    private Vector3 _kittyRespawnLocation;
    private Vector3 _dodgerRespawnLocation;
    [SerializeField] private bool _tutorialPlayer;

    // Start is called before the first frame update
    void Start() {
        // If this is a player pair, get the transforms of
        // Kitty and Dodger as initial checkpoints
        if(_dodger && !_tutorialPlayer) {
            _dodgerRespawnLocation = _dodger.transform.position;
            _kittyRespawnLocation = _kitty.transform.position;
        } else {
            // use the transform of the gameobject this script is attached to
            Debug.LogWarning("Dodger not located! Are we in Tutorial 1?");
            _kittyRespawnLocation = gameObject.transform.position;
        }
    }

    /// <summary>
    /// Called when a checkpoint is reached.
    /// Updates the respawn locations of Kitty and Dodger.
    /// </summary>
    public void UpdateCheckpoint(Checkpoint c) {
        this._kittyRespawnLocation = c.KittyRespawnLocation.transform.position;
        this._dodgerRespawnLocation = c.DodgerRespawnLocation.transform.position;
    }

    /// <summary>
    /// Reset Kitty and Dodger to the previous checkpoint.
    /// </summary>
    public void ResetToCheckpoint() {

        if(!_tutorialPlayer) {
            _kitty.gameObject.transform.position = _kittyRespawnLocation;
            _dodger.gameObject.transform.position = _dodgerRespawnLocation;
            // Debug.Log("Resetting velocity");
            _kitty.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            _dodger.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        } else {
            // This is the tutorial player, so do it a bit differently
            this.gameObject.transform.position = _kittyRespawnLocation;
        }
    }
}
