using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CKWheel : MonoBehaviour
{
    public float rotationSpeed = 45f;   // The speed of the rotation in degrees per second

    void Update()
    {
        // Calculate the amount of rotation for this frame
        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        // Apply the rotation to the object's y-axis
        transform.Rotate(-Vector3.forward, rotationThisFrame);
    }
}
