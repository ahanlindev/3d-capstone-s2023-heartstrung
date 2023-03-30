using UnityEngine;

/// <summary>
/// Attach this script to the "Player Pair" gameObject.
/// Manages the respawn location of the player if they die.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine _kitty;
    [SerializeField] private HeartStateMachine _dodger;
    
    private Vector3 _kittyRespawnLocation;
    private Vector3 _dodgerRespawnLocation;

    // Start is called before the first frame update
    private void Start() {
        // Get initial checkpoint from initial position(s)
        _kittyRespawnLocation = _kitty.transform.position;

        if (!_dodger) { return; }

        _dodgerRespawnLocation = _dodger.transform.position;
    }

    /// <summary>
    /// Called when a checkpoint is reached.
    /// Updates the respawn locations of Kitty and Dodger.
    /// </summary>
    public void UpdateCheckpoint(Checkpoint c) {
        _kittyRespawnLocation = c.KittyRespawnLocation.transform.position;
        _dodgerRespawnLocation = c.DodgerRespawnLocation.transform.position;
    }
    
    /// <summary>
    /// Reset Kitty and Dodger to the previous checkpoint.
    /// </summary>
    public void ResetToCheckpoint() {

        // reset Kitty
        _kitty.transform.position = _kittyRespawnLocation;
        _kitty.rbody.velocity = new Vector3(0f, 0f, 0f);
        _kitty.ChangeState(_kitty.idleState);
        
        // reset Dodger if they exist
        if (!_dodger) { return; }
        _dodger.transform.position = _dodgerRespawnLocation;
        _dodger.rbody.velocity = new Vector3(0f, 0f, 0f);
        _dodger.health.ChangeHealth(_dodger.health.MaxHealth);
        _dodger.ChangeState(_dodger.idleState);
    }
}
