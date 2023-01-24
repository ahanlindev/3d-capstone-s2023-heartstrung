using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary> stores the active state of a button.
public class Button : Trigger
{
    new public void enable() {
        base.enable();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        // gameObject.transform.localScale -= new Vector3(0, .0875f, 0);
    }

    new public void disable() {
        base.disable();
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        // gameObject.transform.localScale += new Vector3(0, .0875f, 0);
    }
}
