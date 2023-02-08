using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuManager : MonoBehaviour
{
    public static OptionsMenuManager instance {get; private set;}

    public GameObject OptionsUI;

    [Tooltip("Whether the game is paused.")]
    [SerializeField] public bool optionsOpen {get; private set;}

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start() {
        optionsOpen = false;
        OptionsUI.SetActive(false);
    }

    public void ChangeOptionsState() {
        if(optionsOpen) {
            CloseOptions();
        } else {
            OpenOptions();
        }
    }

    public void OpenOptions() {
        // Opens the options menu
        optionsOpen = true;
        OptionsUI.SetActive(true);
        PauseMenuManager.instance.HidePauseMenu();
    }

    public void CloseOptions() {
        // Closes the options menu
        optionsOpen = false;
        OptionsUI.SetActive(false);
        if(!PauseMenuManager.instance.pauseBlacklist.Contains(SceneManager.GetActiveScene().name)) {
            PauseMenuManager.instance.ShowPauseMenu();
        }
        // re-enable the title screen if we're on the title screen
        if(SceneManager.GetActiveScene().name == "Main Menu") {
            TitleScreenManager.instance.titleScreen.SetActive(true);
        }
    }
}
