using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//DEBUG
using UnityEngine.EventSystems;

public class ControlsMenuManager : MonoBehaviour
{
    public static ControlsMenuManager instance {get; private set;}

    public GameObject ControlsUI;


    [Tooltip("First button that will be selected when this menu is enabled.")]
    [SerializeField] private UnityEngine.UI.Button defaultSelection;

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

    private void Update() {
        if (controlsOpen && EventSystem.current.currentSelectedGameObject == null) {
            defaultSelection?.Select();
        }
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

        if (!defaultSelection) { Debug.LogError("Options menu has no default button selected!"); }
        defaultSelection.Select();
    }

    public void CloseControls() {
        // Closes the options menu
        controlsOpen = false;
        ControlsUI.SetActive(false);
        AudioManager.instance.playSoundEvent("UIButtonPress");
        PauseMenuManager.instance.ShowPauseMenu();
    }
}
