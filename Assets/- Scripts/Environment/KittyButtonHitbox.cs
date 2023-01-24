using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary> A script placed on the hitbox of a button. It sends signals to the Button script when Kitty steps on and off of it.
public class KittyButtonHitbox : MonoBehaviour
{
    public Button button;
    [Tooltip("If enabled, once pressed the button will stay pressed permanently.")]
    [SerializeField] public bool persistent;
    bool pressed;

    void OnCollisionEnter(Collision collision) {
        if(!pressed) {
            Debug.Log("collided with " + collision.gameObject.name);
            if(collision.gameObject.name == "Kitty") { button.enable(); }
            pressed = true;
        }
    }

    void OnCollisionExit(Collision collision) {
        if(!persistent) {
            Debug.Log("No longer colliding with " + collision.gameObject.name);
            if(collision.gameObject.name == "Kitty") { button.disable(); }
            pressed = false;
        }
    }
}
