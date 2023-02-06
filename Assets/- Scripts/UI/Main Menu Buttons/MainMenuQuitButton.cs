using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuQuitButton : MonoBehaviour
{
    public UnityEngine.UI.Button QuitButton;
    // Start is called before the first frame update
    void Start()
    {
        // QuitButton = GetComponent<UnityEngine.UI.Button>();
        QuitButton.onClick.AddListener(Clicked);
    }

    void Clicked() {
        Debug.Log("Clicked quit button");
        ButtonManager.instance.QuitButtonClicked();
    }
}
