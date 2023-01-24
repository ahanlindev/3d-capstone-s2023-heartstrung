using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("The trigger that opens this door. For buttons this is stored in the ButtonTrigger gameObject.")]
    [SerializeField] public Trigger trigger;
    public MeshRenderer DoorMeshRenderer;
    public MeshCollider DoorMeshCollider;
    
    // Update is called once per frame
    void Update()
    {
        if(trigger.getStatus()) {
            DoorMeshRenderer.enabled = false;
            DoorMeshCollider.enabled = false;
        } else {
            DoorMeshRenderer.enabled = true;
            DoorMeshCollider.enabled = true;
        }
    }
}
