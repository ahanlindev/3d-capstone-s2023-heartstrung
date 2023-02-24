using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance {get; private set;}

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    public void StartGameButtonClicked() {
        // Debug.Log("Let us Start the Game");
        AudioManager.instance.playSoundEvent("ButtonPress");
        TransitionManager.TransitionToScene(SceneID.COMIC1);
    }

    public void OptionsButtonClicked() {
        // Debug.Log("Options GO!");
        AudioManager.instance.playSoundEvent("ButtonPress");
        OptionsMenuManager.instance.ChangeOptionsState();
    }

    public void ControlsButtonClicked() {
        // Debug.Log("Controls GO!");
        AudioManager.instance.playSoundEvent("ButtonPress");
        ControlsMenuManager.instance.ChangeControlsState();
    }

    public void UnpauseButtonClicked() {
        // Debug.Log("Unpaused");
        AudioManager.instance.playSoundEvent("ButtonPress");
        PauseMenuManager.instance.Unpause();
    }

    public void ResetButtonClicked() {
        // Debug.Log("Reset");
        CheckpointManager manager = FindObjectsOfType<CheckpointManager>()[0];
        AudioManager.instance.playSoundEvent("ButtonPress");
        manager.ResetToCheckpoint();
        PauseMenuManager.instance.Unpause();
    }

    public void MainMenuButtonClicked() {
        Debug.Log("Main Menu GO!");
        AudioManager.instance.playSoundEvent("ButtonPress");
        PauseMenuManager.instance.Unpause();
        TransitionManager.TransitionToScene(SceneID.MAINMENU);
    }

    public void QuitButtonClicked() {
        Debug.Log("Quitting Game...");
        AudioManager.instance.playSoundEvent("ButtonPress");
        Application.Quit();
    }
}
