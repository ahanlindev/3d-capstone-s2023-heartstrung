using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>Resets to checkpoint when hit</summary>
public class DeathPitCollider : MonoBehaviour
{
    private CheckpointManager _checkpointManager;
    private string[] _playerTags;
    
    private void Start()
    {
        _playerTags = new [] { "Kitty", "Player", "Heart" };
        
        _checkpointManager = FindObjectOfType<CheckpointManager>();
    }
    
    // works as a barrier that is hit
    private void OnCollisionEnter(Collision col)
    {
        TryResetToCheckpoint(col.gameObject);
    }

    // works as a volume that's safe to exist within
    private void OnTriggerExit(Collider col) {
        TryResetToCheckpoint(col.gameObject);
    }

    /// <summary>
    /// Reset to checkpoint if the target matches one of the necessary tags
    /// </summary>
    /// <param name="target">Object to check against</param>
    private void TryResetToCheckpoint(GameObject target)
    {
        // only go to checkpoint if tag matches
        bool tagMatches = _playerTags.Any(target.CompareTag);
        if (!tagMatches) { return; }
        
        _checkpointManager.ResetToCheckpoint();
    }
}
