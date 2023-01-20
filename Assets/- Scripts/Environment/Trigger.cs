using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    bool isActivated = false;

    public bool getStatus() {
        return isActivated;
    }

    public void enable() {
        isActivated = true;
    }

    public void disable() {
        isActivated = false;
    }
}
