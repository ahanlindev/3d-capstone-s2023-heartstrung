using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ConfigurableButton : MonoBehaviour
{


    [Tooltip("Disappear totally after being triggered")]
    [SerializeField] public bool disappear;

    [Tooltip("If enabled, once pressed the button will stay pressed permanently.")]
    [SerializeField] public bool persistent;

    //[Tooltip("By claiming the attached pf, the objects on the button will be sticked and move along pf's move")]
    //[SerializeField] public PfMovingList attachedPf;
    public enum TriggeringObject {Player, Heart };

    [Tooltip("What will trigger this button?")]
    public TriggeringObject triggeringObject;

    //[Tooltip("It is itself by default.If assigned other, the button logic will be triggered with other conditions")]
    private Button _button;

    [Tooltip("Targets connected with")]
    public List<GameObject> targets;

    private bool _pressed;

    [Tooltip("If enabled, the activeness of this whole object will be switched with this button")]
    [SerializeField] public bool activeSwitch;

    [Tooltip("If enabled, the object renderer will be switched with this button")]
    [SerializeField] public bool visibleSwitch;

    [Tooltip("If enabled, the object collider will be switched with this button")]
    [SerializeField] public bool colliderSwitch;

    [Tooltip("If enabled, the script attached will be switched with this button")]
    [SerializeField] public bool functionalitySwitch;

    /// <summary>color that the button is when the game begins. Used in tweening.</summary>
    private Color _originalColor;
    private Renderer _buttonBodyRenderer;
    //private HashSet<Rigidbody> collidedBodies;


    private void Start()
    {
        //collidedBodies = new HashSet<Rigidbody>();
        if (_button == null)
        {
            _button = gameObject.GetComponent<Button>();
        }
        _buttonBodyRenderer = GetComponent<Renderer>();
        _originalColor = _buttonBodyRenderer.material.color;
    }


    void OnCollisionEnter(Collision collision)
    {
    
        //if (collidedBodies == null) Start();
        //var temp = collision.collider.attachedRigidbody;
        //if (temp != null) collidedBodies.Add(temp);
        //Debug.Log("collided11 with " + collision.gameObject.name);
        if (_button == null)
        {
            Start();
        }
        if (!_pressed)
            {
                Debug.Log("collided with " + collision.gameObject.name);
                if (collision.gameObject.tag == triggeringObject.ToString())
                {
                    _pressed = true;
                    PressTween();
                    if (disappear)
                    {
                        GetComponent<MeshRenderer>().enabled = false;
                        GetComponent<MeshCollider>().enabled = false;
                    }
                }
            }
        
    }

    void OnCollisionExit(Collision collision)
    {
   
        
            //var temp = collision.collider.attachedRigidbody;
            //if (temp != null) collidedBodies.Remove(temp);

            if (!persistent)
            {
                Debug.Log("No longer colliding with " + collision.gameObject.name);
                if (collision.gameObject.name == triggeringObject.ToString())
                {
                    _pressed = false;
                    PressTween();
                }
            }
        
    }

    // Enable to called from outside
    public void DoTrigger()
    {
        foreach (GameObject target in targets)
        {
            if (activeSwitch) Activeness(target);
            if (visibleSwitch) Visibility(target);
            if (functionalitySwitch) Functionality(target);
            if (colliderSwitch) Collision(target);
        }
    }

    // Since fingevent require a float
    public void DoTrigger(float parameter)
    {
        DoTrigger();
    }

    private void Activeness(GameObject target)
    {
        target.SetActive(!target.activeInHierarchy);
    }

    private void Visibility(GameObject target)
    {
        MeshRenderer[] _meshes = target.GetComponentsInChildren<MeshRenderer>();

        // Can't use that question mark but Idky
        foreach (MeshRenderer mesh in _meshes) { mesh.enabled = !mesh.enabled; }
    }

    private void Collision(GameObject target)
    {
        MeshCollider[] _meshColliders = target.GetComponentsInChildren<MeshCollider>();
        Collider[] _colliders = target.GetComponentsInChildren<Collider>();

        foreach (MeshCollider meshCollider in _meshColliders) { meshCollider.enabled = !meshCollider.enabled; }
        foreach (Collider collider in _colliders) { collider.enabled = !collider.enabled; }
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
    
    private void PressTween() {
        const float PRESS_DURATION = 0.25f;

        // fade color 
        Color newColor = _originalColor * .6f;
        _buttonBodyRenderer.material.DOColor(newColor, PRESS_DURATION);
        
        // "press" button
        Vector3 newScale = transform.localScale;
        newScale.y *= .5f;
        
        transform.DOScale(newScale, PRESS_DURATION)
            .OnComplete(DoTrigger);
    }

    private void UnpressTween() {
        const float PRESS_DURATION = 0.25f;

        // restore color 
        _buttonBodyRenderer.material.DOColor(_originalColor, PRESS_DURATION);

        // "unpress" button
        Vector3 newScale = transform.localScale;
        newScale.y *= 2f;

        transform.DOScale(newScale, PRESS_DURATION)
            .OnComplete(DoTrigger);
    }

}

