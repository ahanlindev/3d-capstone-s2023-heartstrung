using UnityEngine;

/// <summary>
/// Used to mark the end of a level
/// </summary>
public class VictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player")) { TransitionManager.TransitionToNextScene(); }
    }
}