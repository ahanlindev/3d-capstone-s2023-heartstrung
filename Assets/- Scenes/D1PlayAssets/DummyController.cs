using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DmmyController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController cc;
    public float moveSpeed;
    public float jumpSpeed;
    public float pulse;

    private float horizontal, vertical;
    private Vector3 dir;
    private Vector3 v;
    public float g;

    public Transform groundCheck;

    public float checkRadius;
    public LayerMask groundLayer;
    public float uplimit;
    private bool isGround;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);
        // returns a float of the hand trigger¡¯s current state on the controller
        // specified by the controller variable.


        if (isGround && v.y < 0)
        {
            v.y = 0;
        }

        horizontal = Input.GetAxis("Horizontal") * moveSpeed;
        vertical = Input.GetAxis("Vertical") * moveSpeed;
        dir = transform.forward * vertical + transform.right * horizontal;
        cc.Move(dir * Time.deltaTime);



        v.y -= g * Time.deltaTime;
        if (Input.GetButton("Jump"))
        {
            if (v.y <= uplimit)
            {
                v.y += jumpSpeed * Time.deltaTime;

            }
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (v.y <= uplimit)
            {
                v.y += pulse;
            }
        }
        cc.Move(v * Time.deltaTime);

    }
}
