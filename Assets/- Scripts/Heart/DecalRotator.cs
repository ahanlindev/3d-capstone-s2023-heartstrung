using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Small script designed to keep an object oriented in the same direction, regardless of its parent rotation
/// </summary>
public class DecalRotator : MonoBehaviour
{
    private Quaternion _rot;
    private Vector3 _offsetFromParent;

    // Start is called before the first frame update
    void Start()
    {
        _rot = transform.rotation;    
        _offsetFromParent = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = transform.parent.position + _offsetFromParent;
        transform.SetPositionAndRotation(newPos, _rot);
    }
}
