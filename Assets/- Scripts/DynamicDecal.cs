using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Small script designed to update a decal projector to match desired parameters.
/// This includes maintaining global rotation and limiting projection depth where necessary.
/// </summary>
[RequireComponent(typeof(DecalProjector))]
public class DynamicDecal : MonoBehaviour
{
    
    [Tooltip("If true, global rotation will be updated to remain consistent with its state on start")]
    [SerializeField] private bool _dynamicRotation = true;

    [Tooltip("If true, projection depth will be updated when the projector hits something, so that it doesn't show " +
             "up on surfaces underneath. This value is capped at the projection depth set at the start.")]
    [SerializeField] private bool _dynamicDepth = true;

    [Tooltip("List of layers that the decal should use to limit depth. Note: Only used when dynamicDepth is enabled")]
    [SerializeField] private LayerMask _blockingLayers;

    [Tooltip("Fixed amount of extra depth added in addition to the dynamic depth. Note: Only used when dynamicDepth is enabled")]
    [SerializeField] private float _bufferDepth = 0.01f;
    
    private LayerMask _actualIgnoredLayers;
    private DecalProjector _projector;
    private Quaternion _rot;
    private Vector3 _offsetFromParent;
    private float _maxDepth;

    // Start is called before the first frame update
    void Start()
    {
        _projector = GetComponent<DecalProjector>();
        _rot = transform.rotation;    
        _offsetFromParent = transform.localPosition;
        _maxDepth = _projector.size.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (_dynamicRotation) { UpdateRotation(); }
        if (_dynamicDepth) { UpdateDepth(); }
    }

    private void UpdateRotation()
    {
        Vector3 newPos = transform.parent.position + _offsetFromParent;
        transform.SetPositionAndRotation(newPos, _rot);
    }
    
    /// <summary>
    /// Ensure that the decal only displays on the first object it hits, by limiting the projection depth 
    /// </summary>
    private void UpdateDepth()
    {
        // Updating depth consists of changing the size of the projection volume,
        // and updating the pivot to half of the projection depth (z value). This
        // maintains the origin of the projection at the origin of the GameObject
        
        
        // cast outward to see if anything is hit
        bool hit = Physics.Raycast(
            origin: transform.position,
            direction: transform.forward, // projection direction is -y of transform
            maxDistance: _maxDepth,
            hitInfo: out RaycastHit hitInfo,
            layerMask: _blockingLayers.value,
            queryTriggerInteraction: QueryTriggerInteraction.Ignore
        );
        
        // calculate actual distance
        float projectionDepth = (hit) ? hitInfo.distance : _maxDepth;
        projectionDepth += _bufferDepth; 
        
        // update and apply new property values
        Vector3 newSize = _projector.size;
        Vector3 newPivot = _projector.pivot;
        
        
        // early return if size is about the same
        if (Mathf.Approximately(newSize.z, projectionDepth)) { return;}
        
        newSize.z = projectionDepth;
        newPivot.z = projectionDepth / 2.0f;

        _projector.size = newSize;
        _projector.pivot = newPivot;
    }
}
