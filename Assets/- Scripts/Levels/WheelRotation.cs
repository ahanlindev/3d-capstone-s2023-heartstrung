using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;
    [SerializeField]
    public bool clockwise = false;


    // Update is called once per frame
    void Update()
    {
        if (clockwise)
            rotationSpeed = -rotationSpeed;
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
