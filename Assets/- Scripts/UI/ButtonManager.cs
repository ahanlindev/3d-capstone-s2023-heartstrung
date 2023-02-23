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
        TransitionManager.TransitionToScene(SceneID.COMIC1);
    }

    public void OptionsButtonClicked() {
        // Debug.Log("Options GO!");
        OptionsMenuManager.instance.ChangeOptionsState();
    }

    public void ControlsButtonClicked() {
        // Debug.Log("Controls GO!");
        ControlsMenuManager.instance.ChangeControlsState();
    }

    public void UnpauseButtonClicked() {
        // Debug.Log("Unpaused");
        PauseMenuManager.instance.Unpause();
    }

    public void ResetButtonClicked() {
        // Debug.Log("Reset");
        CheckpointManager manager = FindObjectsOfType<CheckpointManager>()[0];
        manager.ResetToCheckpoint();
        PauseMenuManager.instance.Unpause();
    }

    public void MainMenuButtonClicked() {
        Debug.Log("Main Menu GO!");
        PauseMenuManager.instance.Unpause();
        TransitionManager.TransitionToScene(SceneID.MAINMENU);
    }

    public void QuitButtonClicked() {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
