using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Component that displays a trajectory path, given 
/// the positions of the start point, pivot, and destination
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    [Tooltip("Number of points that compose the line renderer")]
    [SerializeField] private int _resolution = 100;


    private LineRenderer _renderer;
    private Vector3[] _positions;

    private void Awake() {
        _renderer = GetComponent<LineRenderer>();
        _renderer.enabled = false; // only show trajectory when explicitly told to
        RefreshResolution();
    }

    /// <summary>Update the trajectory that this component will model</summary>
    /// <param name="pivot">Position around which the trajectory will be centered</param>
    /// <param name="localStartPos">Starting position relative to pivot of the flung object</param>
    /// <param name="localDestPos">Desired end position relative to pivot of the flung object</param>
    /// <param name="objRadius"> Radius of the thrown object. Used for detecting 
    /// premature collisions. By default, assumes object has no volume.
    /// </param>
    public void UpdateTrajectory(Vector3 pivot, Vector3 localStartPos, Vector3 localDestPos, float objRadius = 0.0f) {
        RefreshResolution();

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
        _positions[0] = pivot + localStartPos;

        // calculate appropriate points along the line
        for (int i = 1; i < _resolution; i++) {
            float currentRadius = Mathf.Lerp(startRad, destRad, ((float)i / _resolution));

            Vector3 pivotDirToPoint = (_positions[i-1] - pivot).normalized;
            pivotDirToPoint = Quaternion.AngleAxis(degPerTick, axis) * pivotDirToPoint;
            
            _positions[i] = pivot + (pivotDirToPoint * currentRadius);

            // TODO spherecast to check for early collision
        }

        // Apply changes to renderer
        _renderer.SetPositions(_positions);
    }

    // <summary>Toggle whether the trajectory line will be rendered</summary>
    public void ToggleRender(bool enabled) => _renderer.enabled = enabled;

    /// <summary>Set renderer resolution to whatever is currently set in editor</summary>
    private void RefreshResolution() {
        if (_renderer.positionCount != _resolution) {
            _renderer.positionCount = _resolution; 
            Array.Resize(ref _positions, _resolution);
        }
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
