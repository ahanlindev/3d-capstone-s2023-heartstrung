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
            _dodgerRespawnLocation = new Vector3(
                _dodger.transform.position.x,
                _dodger.transform.position.y,
                _dodger.transform.position.z);
            _kittyRespawnLocation = new Vector3(
                _kitty.transform.position.x,
                _kitty.transform.position.y,
                _kitty.transform.position.z);
        } else {
            // use the transform of the gameobject this script is attached to
            Debug.LogWarning("Dodger not located! Are we in Tutorial 1?");
            _kittyRespawnLocation = new Vector3(
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
        this._kittyRespawnLocation = c.KittyRespawnLocation.transform.position;
        this._dodgerRespawnLocation = c.DodgerRespawnLocation.transform.position;
    }

    /// <summary>
    /// Reset Kitty and Dodger to the previous checkpoint.
    /// </summary>
    public void ResetToCheckpoint() {
        Debug.Log("Resetting to last checkpoint...");

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
