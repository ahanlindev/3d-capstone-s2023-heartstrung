using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMPF : MonoBehaviour
{

    [SerializeField] public Trigger trigger;
    ConfigurableMPF MPFScript;

    // Start is called before the first frame update
    void Start()
    {
        MPFScript = transform.GetChild(0).GetComponent<ConfigurableMPF>();
        MPFScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!MPFScript.enabled)
        {
            if (trigger.getStatus())
            {
                MPFScript.enabled = true;
            }
            else
            {
                MPFScript.enabled = false;
            }
        }
      
    }
}
