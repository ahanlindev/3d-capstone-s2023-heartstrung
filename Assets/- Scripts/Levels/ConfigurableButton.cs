using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurableButton : MonoBehaviour
{

    public enum TriggeringObject {Player, Heart };

    [Tooltip("What will trigger this button?")]
    public TriggeringObject triggeringObject;

    //[Tooltip("It is itself by default.If assigned other, the button logic will be triggered with other conditions")]
    private Button _button;

    [Tooltip("Targets connected with")]
    public List<GameObject> targets;

    [Tooltip("If enabled, once pressed the button will stay pressed permanently.")]
    [SerializeField] public bool persistent;

    private bool _pressed;

    [Tooltip("If enabled, the activeness of this whole object will be switched with this button")]
    [SerializeField] public bool activeSwitch;

    [Tooltip("If enabled, the object renderer will be switched with this button")]
    [SerializeField] public bool visibleSwitch;

    [Tooltip("If enabled, the script attached will be switched with this button")]
    [SerializeField] public bool functionalitySwitch;



    private void Start()
    {
        if (_button == null)
        {
            _button = gameObject.GetComponent<Button>();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!_pressed)
        {
            Debug.Log("collided with " + collision.gameObject.name);
            if (collision.gameObject.tag == triggeringObject.ToString())
            {
                _button.enable();
                _pressed = true;
                DoTrigger();
            }

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!persistent)
        {
            Debug.Log("No longer colliding with " + collision.gameObject.name);
            if (collision.gameObject.name == triggeringObject.ToString())
            {
                _button.disable();
                _pressed = false;
                DoTrigger();
            }
        }
    }

    private void DoTrigger()
    {
        foreach (GameObject target in targets)
        {
            if (activeSwitch) Activeness(target);
            if (visibleSwitch) Visibility(target);
            if (functionalitySwitch) Functionality(target);
        }
    }

    private void Activeness(GameObject target)
    {
        target.SetActive(!target.activeInHierarchy);
    }

    private void Visibility(GameObject target)
    {
        MeshRenderer[] _meshes = target.GetComponentsInChildren<MeshRenderer>();
        MeshCollider[] _meshColliders = target.GetComponentsInChildren<MeshCollider>();
        Collider[] _colliders = target.GetComponentsInChildren<Collider>();

        // Can't use that question mark but Idky
        foreach (MeshRenderer mesh in _meshes) mesh.enabled = !mesh.enabled;

        foreach (MeshCollider meshCollider in _meshColliders) meshCollider.enabled = !meshCollider.enabled;

        foreach (Collider collider in _colliders) collider.enabled = !collider.enabled;
    }

    private void Functionality(GameObject target)
    {
        MonoBehaviour[] scripts = target.GetComponentsInChildren<MonoBehaviour>();
     
        foreach (MonoBehaviour script in scripts)
        {
            Debug.Log("Change state " + script.name);
            script.enabled = !script.enabled;
        }
    }
}

