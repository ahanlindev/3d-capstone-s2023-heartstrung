using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Component that displays a trajectory path, given 
/// the positions of the start point, pivot, and destination
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class FlingTrajectory : MonoBehaviour
{
    [Tooltip("Number of points that compose the line renderer")]
    [SerializeField] private int _resolution = 100;

    private LineRenderer _renderer;
    private Vector3[] _positions;

    private void Awake() {
        _renderer.enabled = false; // only show trajectory when explicitly told to
        RefreshResolution();
    }

    /// <summary>Update the trajectory that this component will model</summary>
    /// <param name="pivot">Position around which the trajectory will be centered</param>
    /// <param name="startPos">Starting position of the flung object</param>
    /// <param name="destination">Desired end position of the flung object</param>
    /// <param name="objRadius"> Radius of the thrown object. Used for detecting 
    /// premature collisions. By default, assumes object has no volume.
    /// </param>
    public void UpdateTrajectory(Vector3 pivot, Vector3 startPos, Vector3 destination, float objRadius = 0.0f) {
        RefreshResolution();
        
        // calculate appropriate points along the line
        for (int i = 0; i < _resolution; i++) {

        }
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
}
