using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float floatingSpeed = 0.5f;
    private float startingY;

    void Start()
    {
        startingY = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        transform.position = new Vector3(transform.position.x, startingY + Mathf.Sin(Time.time * floatingSpeed) * 0.5f, transform.position.z);
    }
}
