using UnityEngine;
using UnityEngine.Serialization;

public class TutorialTrigger : MonoBehaviour
{

    [FormerlySerializedAs("hintIndex")] 
    [SerializeField] private int _hintIndex;

    [FormerlySerializedAs("countDown")]
    [Tooltip("The time kitty need to pass before triggering the bubble")]
    [SerializeField] private int _countDown;

    [FormerlySerializedAs("tutorialBubbles")]
    [Tooltip("Targeted UI Bubbles, if empty, will search for name [ChatBubble]")]
    [SerializeField] public TutorialBubble[] _tutorialBubbles;

    [Tooltip("If true, this tutorial will be triggered by Dodger instead of Kitty")]
    [SerializeField] private bool _detectDodger;


    // Start is called before the first frame update
    private void Start()
    {
        if (_tutorialBubbles.Length == 0)
        {
            // get the only one by name
            _tutorialBubbles = GameObject.Find("ChatBubble").GetComponents<TutorialBubble>();
        }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        string triggerTag = (_detectDodger) ? "Heart" : "Player";
        if (other.gameObject.tag == triggerTag)
        {
            if (_countDown == 0)
            {
                foreach (TutorialBubble tutorialBubble in _tutorialBubbles)
                {
                    //Debug.Log("11");
                    tutorialBubble?.changeText(_hintIndex);
                }
            }     
        }

    }

    private void OnTriggerExit(Collider other)
    {
        string triggerTag = (_detectDodger) ? "Heart" : "Player";
        if (other.gameObject.tag == triggerTag)
        {
            _countDown--;
            foreach (TutorialBubble tutorialBubble in _tutorialBubbles)
            {
                tutorialBubble?.checktheBush(_hintIndex);
                tutorialBubble?.cleanText();
                //Tween lastcall = DOVirtual.DelayedCall(3f, () => tutorialBubble.cleanText(), false);
            }
        }
    }

}
