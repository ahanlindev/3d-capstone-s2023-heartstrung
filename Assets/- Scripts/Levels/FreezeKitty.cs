using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeKitty : MonoBehaviour
{
    // This script is currently deprecated
    // The logic of this script is that we can disable this at the begining and trigger by a button;
    // Start is called before the first frame update
    [Tooltip("Attach playerStateMachine in Kitty here")]
    public PlayerStateMachine playerStateMachine;

    [Tooltip("The triggering of this script will change the speed to following value")]
    public float desiredSpeed;

    private bool triggered;
    void Start()
    {
        //playerStateMachine.moveSpeed = desiredSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!triggered) Start();
    }
}
