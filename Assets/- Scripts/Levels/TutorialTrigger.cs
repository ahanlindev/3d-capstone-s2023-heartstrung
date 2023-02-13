using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialTrigger : MonoBehaviour
{

    public int HintIndex;
    TutorialBubble tutorialBubble;
    

    // Start is called before the first frame update
    void Start()
    {
        tutorialBubble = GameObject.Find("ChatBubble").GetComponent<TutorialBubble>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tutorialBubble.changeText(HintIndex);
        }

    }

    private void OnTriggerExit(Collider other)
    {
    
        if (other.gameObject.tag == "Player")
        {
            tutorialBubble.checktheBush(HintIndex);
            DOVirtual.DelayedCall(2f, () => tutorialBubble.cleanText(), false);
        }
    }

}
