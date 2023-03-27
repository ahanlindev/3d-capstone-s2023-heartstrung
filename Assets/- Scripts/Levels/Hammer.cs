using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public float swingAngle = 30f;      // The maximum angle the hammer will swing in degrees
    public float swingSpeed = 1f;       // The speed of the swing in seconds per full swing

    private float swingDirection = 1f;  // The direction of the swing (either 1 or -1)
    private float swingTime = 0f;       // The time elapsed since the start of the swing

    void FixedUpdate()
    {
        // Calculate the angle of the swing using time and swing speed
        float swingAngleThisFrame = Mathf.Sin(swingTime * Mathf.PI * 2f / swingSpeed) * swingAngle;

        // Calculate the rotation angle needed to match the swing angle
        float rotationAngle = swingAngleThisFrame - transform.localEulerAngles.z;

        // Rotate the hammer to match the swing angle
        transform.Rotate(Vector3.forward, rotationAngle);

        // Advance the swing time and direction
        swingTime += Time.deltaTime * swingDirection;

    }
}