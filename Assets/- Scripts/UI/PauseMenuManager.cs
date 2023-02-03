using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance {get; private set;}
    public static Action PauseEvent;
    public static Action UnpauseEvent;
    private PlayerInput inputActions;
    public GameObject pauseUI;

    [Tooltip("A list of scenes where the pause menu cannot be enabled.")]
    [SerializeField] public List<string> pauseBlacklist = new List<string>();

    [Tooltip("Whether the game is paused.")]
    [SerializeField] private bool paused;
    
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
        pauseUI.SetActive(false);
    }

    private void OnEnable() {
        inputActions.Gameplay.Pause.performed += OnPauseKeyPressed;
    }

    private void OnDisable() {
        inputActions.Gameplay.Pause.performed -= OnPauseKeyPressed;
    }

    // void Start() {
    //     PauseMenuEvent.AddListener(PauseKeyPressed);
    // }

    private void OnPauseKeyPressed(InputAction.CallbackContext _) {
        Debug.Log("Pause");
        // Check if the current scene is in the pause blacklist
        if(!pauseBlacklist.Contains(SceneManager.GetActiveScene().name)) {
            if(!paused) {
                Pause();
            } else {
                Unpause();
            }
        }
    }

    public void Pause() {
        // Pause the game
        paused = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.instance.playSoundEvent("PauseOn");
        PauseEvent?.Invoke();
    }

    public void Unpause() {
        // Unpause the game
        paused = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.playSoundEvent("PauseOff");
        UnpauseEvent?.Invoke();
    }
}
