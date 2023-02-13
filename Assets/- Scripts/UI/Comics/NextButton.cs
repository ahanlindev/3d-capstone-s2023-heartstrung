using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : MonoBehaviour
{
    public UnityEngine.UI.Button Button;
    public ComicManager Manager;
    // Start is called before the first frame update
    void Start()
    {
        // QuitButton = GetComponent<UnityEngine.UI.Button>();
        Button.onClick.AddListener(Clicked);
    }

    void Clicked() {
        Manager.Next();
    }
}
