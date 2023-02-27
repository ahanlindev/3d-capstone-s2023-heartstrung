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


    private void Start()
    {
        if (button == null)
        {
            button = gameObject.GetComponent<Button>();
        }
    }
    void OnTriggerEnter(Collider collision) {
        if(!pressed) {
            Debug.Log("kittybutton collided with " + collision.gameObject.name);
            if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Kitty") {
                button.enable();
                pressed = true;
            }
            Debug.Log("tag is " + collision.gameObject.tag);
           
        }
    }

    void OnTriggerExit(Collider collision) {
        if(!persistent) {
            Debug.Log("kittybutton no longer colliding with " + collision.gameObject.name);
            if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Kitty") {
                button.disable();
                pressed = false;
            }
            
        }
    }
}
