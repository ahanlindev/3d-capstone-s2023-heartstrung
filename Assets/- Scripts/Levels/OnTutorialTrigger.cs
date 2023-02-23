using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class OnTutorialTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("Target connected with")]
    [SerializeField] public GameObject triggerObject;

    private bool _called;

    public int HintIndex;

    TutorialBubble _tutorialBubble;
    void Start()
    {
        _tutorialBubble = GameObject.Find("ChatBubble").GetComponent<TutorialBubble>();
    }

    // TODO: bad bad code for checking, will switch to event later 
    void Update()
    {
        if (!_called)
            if (triggerObject.activeInHierarchy)
            {
                _tutorialBubble.changeText(HintIndex);
                _called = true;
            }
    }

}
