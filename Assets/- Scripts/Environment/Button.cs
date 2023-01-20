using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Trigger
{
    void OnMouseDown() {
        Debug.Log("Clicked Down");
        this.enable();
    }
}
