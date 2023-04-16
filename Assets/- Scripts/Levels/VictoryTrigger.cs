using UnityEngine;

/// <summary>
/// Used to mark the end of a level
/// </summary>
public class VictoryTrigger : MonoBehaviour
{
    private bool triggered;
    private void OnTriggerEnter(Collider coll)
    {
        if (triggered) { return; }

        triggered = true;

        if (coll.gameObject.tag is "Player" or "Heart")
        {
            TransitionManager.TransitionToNextScene();
        }
    }
}