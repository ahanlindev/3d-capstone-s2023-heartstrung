using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLevelSelectButton : MonoBehaviour
{
    UnityEngine.UI.Button MenuButton;
    // Start is called before the first frame update
    void Start()
    {
        MenuButton = GetComponent<UnityEngine.UI.Button>();
        MenuButton.onClick.AddListener(Clicked);
    }

    void Clicked() {
        ButtonManager.instance.LevelSelectButtonClicked();
    }
}
