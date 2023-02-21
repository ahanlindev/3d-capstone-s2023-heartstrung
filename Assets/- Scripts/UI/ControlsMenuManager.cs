using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenuManager : MonoBehaviour
{
    public static ControlsMenuManager instance {get; private set;}

    public GameObject ControlsUI;

    [Tooltip("Whether the controls panel is open.")]
    [SerializeField] public bool controlsOpen {get; private set;}

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start() {
        controlsOpen = false;
        ControlsUI.SetActive(false);
    }

    public void ChangeControlsState() {
        if(controlsOpen) {
            CloseControls();
        } else {
            OpenControls();
        }
    }

    public void OpenControls() {
        // Opens the controls menu
        controlsOpen = true;
        ControlsUI.SetActive(true);
        PauseMenuManager.instance.HidePauseMenu();
        OptionsMenuManager.instance.HideOptionsMenu();
    }

    public void CloseControls() {
        // Closes the options menu
        controlsOpen = false;
        ControlsUI.SetActive(false);
        PauseMenuManager.instance.ShowPauseMenu();
    }
}
