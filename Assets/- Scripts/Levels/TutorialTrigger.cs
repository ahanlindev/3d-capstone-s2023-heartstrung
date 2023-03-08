using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialTrigger : MonoBehaviour
{

    public int hintIndex;

    [Tooltip("The time kitty need to pass before triggering the bubble")]
    public int countDown;

    [Tooltip("Targeted UI Bubbles, if empty, will search for name [ChatBubble]")]
    [SerializeField] public TutorialBubble[] tutorialBubbles;




    // Start is called before the first frame update
    void Start()
    {
        if (tutorialBubbles.Length == 0)
        {
            // get the only one by name
            tutorialBubbles = GameObject.Find("ChatBubble").GetComponents<TutorialBubble>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (countDown == 0)
            {
                foreach (TutorialBubble tutorialBubble in tutorialBubbles)
                {
                    //Debug.Log("11");
                    tutorialBubble?.changeText(hintIndex);
                }
            }     
        }

    }

    private void OnTriggerExit(Collider other)
    {
    
        if (other.gameObject.tag == "Player")
        {
            countDown--;
            foreach (TutorialBubble tutorialBubble in tutorialBubbles)
            {
                tutorialBubble.checktheBush(hintIndex);
                tutorialBubble.cleanText();
                //Tween lastcall = DOVirtual.DelayedCall(3f, () => tutorialBubble.cleanText(), false);
            }
        }
    }

}
