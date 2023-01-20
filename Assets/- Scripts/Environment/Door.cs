using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Trigger trigger;
    bool open = false;

    // Update is called once per frame
    void Update()
    {
        if(open) {
            Debug.Log("Door Opened.");
            Destroy(this.gameObject);
        } else {
            checkForOpen();
        }
    }

    void checkForOpen() {
        if(trigger.getStatus()) { open = true; }
    }
}
