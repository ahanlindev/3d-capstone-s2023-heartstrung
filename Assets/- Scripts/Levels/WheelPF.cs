using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPF : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Hanger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Hanger.position;
    }
}
