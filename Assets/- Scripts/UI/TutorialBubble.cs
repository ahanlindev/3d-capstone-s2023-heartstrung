using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBubble : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Kitty;
    public Transform VirtualCamera;
    private Vector3 shift;
    void Start()
    {
        shift = transform.position - Kitty.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Kitty.transform.position + shift;
        Vector3 forwardDirection = (transform.position - VirtualCamera.transform.position).normalized;
        transform.LookAt(transform.position + forwardDirection, VirtualCamera.transform.up);
    }
}
