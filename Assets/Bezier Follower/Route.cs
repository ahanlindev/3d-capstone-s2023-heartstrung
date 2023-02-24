using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour{
[SerializeField]
private List<Transform> controlPoints;

private Vector2 gizmosPos;

private void OnValidate() {
    if (!controlPoints) { 
        controlPoints = new List<Transform>();
    }
Debug.Log($"controlPoints number: {controlPoints.Count()}");
}

private void OnDrawGizmos()
{
    for(float t = 0; t <= 1; t += 0.05f)
    {
        gizmosPos = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
            3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
            3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
            Mathf.Pow(t, 3) * controlPoints[3].position;

        Gizmos.DrawSphere(gizmosPos, 0.25f);
    }

    Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
        new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));

    Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
       new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
}}