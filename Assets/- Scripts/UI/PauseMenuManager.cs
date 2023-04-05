using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance {get; private set;}
    public static Action PauseEvent;
    public static Action UnpauseEvent;
    private PlayerInput inputActions;
    public GameObject pauseUI;

    [Tooltip("First button that will be selected when this menu is enabled.")]
    [SerializeField] private UnityEngine.UI.Button defaultSelection;

    [Tooltip("A list of scenes where the pause menu cannot be enabled.")]
    [SerializeField] public List<SceneID> pauseBlacklist = new List<SceneID>();

    [Tooltip("Whether the game is paused.")]
    [SerializeField] public bool paused {get; private set;}
    
    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        // initialize input
        inputActions = new PlayerInput();
        inputActions.Gameplay.Enable();
    }

    void Start() {
        paused = false;
        pauseUI.SetActive(false);
    }

    // TODO I would definitely like to put these menus into a parent class
    // TODO this "check all menus are inactive" is awful and Alex's fault
    private void Update() {
        if (paused && 
            !OptionsMenuManager.instance.optionsOpen &&
            !ControlsMenuManager.instance.controlsOpen &&
            EventSystem.current.currentSelectedGameObject == null) {
            defaultSelection?.Select();
        }
    }


    private void OnEnable() {
        inputActions.Gameplay.Pause.performed += OnPauseKeyPressed;
    }

    // void Start() {
    //     PauseMenuEvent.AddListener(PauseKeyPressed);
    // }

    private void OnPauseKeyPressed(InputAction.CallbackContext _) {
        // Debug.Log("Pause");
        // Check if the current scene is in the pause blacklist
        if(CanPause()) {
            if(!paused) {
                Pause();
            } else {
                // if the options menu is open, close the options menu
                if(OptionsMenuManager.instance.optionsOpen) {
                    OptionsMenuManager.instance.CloseOptions();
                } else {
                    Unpause();
                }
            }
        }
    }

    public void Pause() {
        // Pause the game
        paused = true;
        Time.timeScale = 0f;
        AudioManager.instance?.playSoundEvent("PauseOn");
        AudioManager.instance?.pauseMusic();
        PauseEvent?.Invoke();
        ShowPauseMenu();
    }

    public void Unpause() {
        // Unpause the game
        paused = false;
        Time.timeScale = 1f;
        AudioManager.instance?.playSoundEvent("PauseOff");
        AudioManager.instance?.unPauseMusic();
        UnpauseEvent?.Invoke();
        // to be safe, also close the options menu
        OptionsMenuManager.instance.CloseOptions();
        ControlsMenuManager.instance.CloseControls();
        HidePauseMenu();
    }

    public void ShowPauseMenu() {
        pauseUI.SetActive(true);
        
        if (!defaultSelection) { Debug.LogError("Pause menu has no default button selected!"); }
        defaultSelection.Select();
    }

    public bool CanPause() => !pauseBlacklist.Contains(SceneManager.GetActiveScene().ToSceneID());

    public void HidePauseMenu() {
        pauseUI.SetActive(false);
    }
}
