using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventTutorialTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public int HintIndex;
    TutorialBubble _tutorialBubble;
    private PlayerStateMachine _player;
    private int _trigertime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _tutorialBubble = GameObject.Find("ChatBubble").GetComponent<TutorialBubble>();
        _player = FindObjectsOfType<PlayerStateMachine>()[0];
        _player.FlingEvent += Trigger;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        _player.FlingEvent -= Trigger;
    }

    void Trigger(float power)
    {
        _trigertime++;
        if (_trigertime > 2)
        {
            _tutorialBubble.changeText(HintIndex);
        }
    }

}
