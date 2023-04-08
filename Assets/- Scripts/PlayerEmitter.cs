using UnityEngine;

/// <summary>
/// Used to decouple the two player characters
/// from each other at runtime while allowing
/// them to be kept as a single prefab in editor
/// </summary>
public class PlayerEmitter : MonoBehaviour
{
    private void Start()
    {
        transform.GetChild(1).SetParent(null);
        transform.GetChild(0).SetParent(null);
    }
}
