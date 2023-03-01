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
    [Tooltip("Number of points that compose the line renderer")]
    [SerializeField] private int _resolution = 32;


    private LineRenderer _renderer;
    private GameObject _floorDecal;

    private void Awake() {
        _renderer = GetComponent<LineRenderer>();
        _renderer.enabled = false; // only show trajectory when explicitly told to

        // get child with decal projector
        _floorDecal = transform.GetComponentInChildren<DecalProjector>().gameObject;
        if (!_floorDecal) {Debug.LogError("Trajectory renderer does not have a decal projector in its children");}
        _floorDecal?.SetActive(false);
    }

    /// <summary>Update the trajectory that this component will model</summary>
    /// <param name="pivot">Position around which the trajectory will be centered</param>
    /// <param name="localStartPos">Starting position relative to pivot of the flung object</param>
    /// <param name="localDestPos">Desired end position relative to pivot of the flung object</param>
    /// <param name="objRadius"> Radius of the thrown object. Used for detecting 
    /// premature collisions. By default, assumes object has no volume.
    /// </param>
    public void UpdateTrajectory(Vector3 pivot, Vector3 localStartPos, Vector3 localDestPos, float objRadius = 0.0f) {
        var positions = new List<Vector3>();

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
        Vector3 decalDirection = Vector3.down;

        // calculate starting point
        positions.Add(pivot + localStartPos);

        // calculate appropriate points along the line
        for (int i = 1; i < _resolution; i++) {
            float currentRadius = Mathf.Lerp(startRad, destRad, ((float)i / _resolution));
            Vector3 lastPos = positions[i-1];
            
            Vector3 pivotDirToPos = (lastPos - pivot).normalized;
            pivotDirToPos = Quaternion.AngleAxis(degPerTick, axis) * pivotDirToPos;
            
            Vector3 currentPos = pivot + (pivotDirToPos * currentRadius); 
            
            Vector3 vecToCurrentPos = (currentPos - lastPos);

            positions.Add(currentPos);

            // spherecast to check for early collision
            Ray ray = new Ray(lastPos, vecToCurrentPos.normalized);
            RaycastHit hitInfo;
            bool willHit = Physics.SphereCast(
                ray: ray, 
                radius: objRadius, 
                maxDistance: vecToCurrentPos.magnitude, 
                hitInfo: out hitInfo,
                layerMask: 1
            );

            // exit the loop if the spherecast hits something
            if (willHit && !hitInfo.collider.isTrigger) { 
                decalDirection = vecToCurrentPos;
                break; 
            }
        }

        // Apply changes to renderer
        _renderer.positionCount = positions.Count;
        _renderer.SetPositions(positions.ToArray());

        
        // get position slightly behind end of trajectory line
        int secondToLastIdx = positions.Count - 2;
        Vector3 decalPos = positions[secondToLastIdx] - (decalDirection.normalized);
        
        // update decal transform
        _floorDecal?.transform.SetPositionAndRotation(
            decalPos, 
            Quaternion.LookRotation(decalDirection)
        );
    }

    // <summary>Toggle whether the trajectory line will be rendered</summary>
    public void ToggleRender(bool enabled) {
        _renderer.enabled = enabled;
        _floorDecal.SetActive(enabled);
    }


    /// <summary>
    /// Update the position and rotation of the decal projector object
    /// </summary>
    private void UpdateDecalTransform(Vector3 position, Quaternion rotation) {
        _floorDecal.transform.SetPositionAndRotation(position, rotation);
    }

    /// <summary>
    /// Gets a normalized vector orthogonal to both parameter vectors. 
    /// Parameters do not need to be orthonormal.</summary>
    /// <param name="normal">the "forward" vector</param>
    /// <param name="tangent">the "upward" vector</param>
    /// <returns>The binormal or "rightward" vector relative to the parameters </returns>
    private Vector3 GetBinormal(Vector3 normal, Vector3 tangent) {
        var orthoNorm = normal;
        var orthoTan = tangent;
        Vector3.OrthoNormalize(ref orthoNorm, ref orthoTan);

        return Vector3.Cross(orthoNorm, orthoTan).normalized;
    }
}
