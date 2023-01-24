using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary> A generic class that stores the binary state of an interactable entity in the level, such as a button or lever.
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
