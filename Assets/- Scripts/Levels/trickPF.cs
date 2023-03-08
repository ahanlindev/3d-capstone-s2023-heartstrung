using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trickPF : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerStateMachine _player;

    [Tooltip("Targets connected with")]
    public List<GameObject> targets;

    private bool _pressed;


    void Start()
    {
        _player = FindObjectsOfType<PlayerStateMachine>()[0];
        _player.ChargeFlingEvent += DoTrigger;

    }

    void OnDestroy()
    {
        _player.ChargeFlingEvent -= DoTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Enable to called from outside
    public void DoTrigger()
    {
        if (!_pressed)
        {
            _pressed = true;
            foreach (GameObject target in targets)
            {
                Functionality(target);
            }
        }
      
    }

    // Since fingevent require a float
    public void DoTrigger(float parameter)
    {
        DoTrigger();
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
