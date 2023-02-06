using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOptionsButton : MonoBehaviour
{
    UnityEngine.UI.Button MenuButton;
    public GameObject TitleScreen;
    // Start is called before the first frame update
    void Start()
    {
        MenuButton = GetComponent<UnityEngine.UI.Button>();
        MenuButton.onClick.AddListener(Clicked);
    }

    void Clicked() {
        ButtonManager.instance.OptionsButtonClicked();
        TitleScreen.SetActive(false);
    }
}
