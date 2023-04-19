using UnityEngine;

/// <summary>
/// Used to mark the end of a level
/// </summary>
public class VictoryTrigger : MonoBehaviour
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider coll)
    {
        if (triggered) { return; }

        if (coll.gameObject.tag is "Player" or "Heart")
        {
            triggered = true;
            TransitionManager.TransitionToNextScene();
        }
    }
}