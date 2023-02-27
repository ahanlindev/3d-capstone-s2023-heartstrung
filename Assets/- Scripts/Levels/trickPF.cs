using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trickPF : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerStateMachine _player;

    [Tooltip("Button effect to trigger")]
    [SerializeField] public ConfigurableButton button;

    void Start()
    {
        _player = FindObjectsOfType<PlayerStateMachine>()[0];
        _player.ChargeFlingEvent += button.DoTrigger;

    }

    void OnDestroy()
    {
        _player.ChargeFlingEvent -= button.DoTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
