using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Component that displays a trajectory path, given 
/// the positions of the start point, pivot, and destination
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    #region Serialized Fields
    
    [Tooltip("Number of points that compose the line renderer")]
    [SerializeField] private int _resolution = 32;

    #endregion
    
    #region Private Fields
    
    private LineRenderer _renderer;
    private List<Vector3> _positions;
    private GameObject _floorDecal;

    #endregion
    
    #region MonoBehaviour Methods
    
    private void Awake() {
        _renderer = GetComponent<LineRenderer>();
        _renderer.enabled = false; // only show trajectory when explicitly told to

        _positions = new List<Vector3>();

        // get child with decal projector
        _floorDecal = transform.GetComponentInChildren<DecalProjector>()?.gameObject;
        if (!_floorDecal) {
            Debug.LogWarning("Trajectory renderer does not have a decal projector in its children");
            this.enabled = false; // prevent update from being called
            return;
        }
        _floorDecal.SetActive(false);
    }

    private void Update() {
        // if decal is enabled, make sure it is synced with the trajectory.
        if (_floorDecal.activeSelf) {
            UpdateDecalTransform();
        }
    }

    #endregion
    
    #region Public Methods
    
    /// <summary>Update the trajectory that this component will model</summary>
    /// <param name="pivot">Position around which the trajectory will be centered</param>
    /// <param name="localStartPos">Starting position relative to pivot of the flung object</param>
    /// <param name="localDestPos">Desired end position relative to pivot of the flung object</param>
    /// <param name="objRadius">The radius of the object. Assumes volume of 0 by default</param>
    public void UpdateTrajectory(Vector3 pivot, Vector3 localStartPos, Vector3 localDestPos, float objRadius = 0.0f) {
        _positions = new List<Vector3>();

        // initialize necessary vector info
        float startRad = localStartPos.magnitude;
        float destRad = localDestPos.magnitude;

        Vector3 axis = GetBinormal(localStartPos, localDestPos);

        // calculate total angle to destination, and fix axis and angle if needed
        float totalAngle = Vector3.Angle(localStartPos, localDestPos);
        if (localStartPos.y < localDestPos.y) { 
            totalAngle = 360.0f - totalAngle; 
            axis = -axis;
        }

        // set up loop variables
        float degPerTick = totalAngle / _resolution;

        // calculate starting point
        _positions.Add(pivot + localStartPos);

        // calculate appropriate points along the line
        for (int i = 1; i < _resolution; i++) {
            float currentRadius = Mathf.Lerp(startRad, destRad, ((float)i / _resolution));
            Vector3 lastPos = _positions[i-1];
            
            Vector3 pivotDirToPos = (lastPos - pivot).normalized;
            pivotDirToPos = Quaternion.AngleAxis(degPerTick, axis) * pivotDirToPos;
            
            Vector3 currentPos = pivot + (pivotDirToPos * currentRadius); 
            
            Vector3 vecToCurrentPos = (currentPos - lastPos);

            _positions.Add(currentPos);

            // spherecast to check for early collision
            var ray = new Ray(lastPos, vecToCurrentPos.normalized);
            bool willHit = Physics.SphereCast(
                ray: ray, 
                radius: objRadius, 
                maxDistance: vecToCurrentPos.magnitude, 
                hitInfo: out RaycastHit hitInfo,
                layerMask: 1
            );

            // exit the loop if the spherecast hits something
            if (willHit && !hitInfo.collider.isTrigger) { 
                break; 
            }
        }

        // Apply changes to renderer
        _renderer.positionCount = _positions.Count;
        _renderer.SetPositions(_positions.ToArray());
    }

    // <summary>Toggle whether the trajectory line will be rendered</summary>
    public void ToggleRender(bool setEnabled) {
        _renderer.enabled = setEnabled;
        _floorDecal.SetActive(setEnabled);
    }
    
    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Update the position and rotation of the decal projector object, based on current
    /// state of the positions array
    /// </summary>
    private void UpdateDecalTransform() {
        // invalid state if positions is empty or decal object is missing
        if (_positions.Count == 0) { return; }
        if (!_floorDecal) { return; }
        
        // use this idx to get position slightly behind end of trajectory line
        int secondToLastIdx = _positions.Count - 2;

        // if renderer was interrupted, point in direction of last link of renderer, 
        // otherwise point straight down
        Vector3 direction = (_positions.Count == _resolution) 
            ? Vector3.down 
            : (_positions.Last() - _positions[secondToLastIdx]).normalized;

        // bump position back a bit to avoid clipping into stuff.
        Vector3 decalPos = _positions[secondToLastIdx] - (direction.normalized);

        // update decal transform
        _floorDecal.transform.SetPositionAndRotation(
            decalPos, 
            Quaternion.LookRotation(direction)
        );
    }

    /// <summary>
    /// Gets a normalized vector orthogonal to both parameter vectors. 
    /// Parameters do not need to be orthonormal.</summary>
    /// <param name="normal">the "forward" vector</param>
    /// <param name="tangent">the "upward" vector</param>
    /// <returns>The binormal or "rightward" vector relative to the parameters </returns>
    private static Vector3 GetBinormal(Vector3 normal, Vector3 tangent) {
        Vector3 orthoNorm = normal;
        Vector3 orthoTan = tangent;
        Vector3.OrthoNormalize(ref orthoNorm, ref orthoTan);

        return Vector3.Cross(orthoNorm, orthoTan).normalized;
    }
    
    #endregion
}
