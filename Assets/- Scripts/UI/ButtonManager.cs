using UnityEngine;

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

    public void LevelSelectButtonClicked() {
        AudioManager.instance.playSoundEvent("ButtonPress");
        TransitionManager.TransitionToScene(SceneID.LEVELSELECT);
    }

    public void LevelButtonClicked(SceneID levelID) {
        AudioManager.instance.playSoundEvent("ButtonPress");
        TransitionManager.TransitionToScene(levelID);
    }

    public void MainMenuButtonClicked() {
        AudioManager.instance.playSoundEvent("ButtonPress");
        PauseMenuManager.instance.Unpause();
        TransitionManager.TransitionToScene(SceneID.MAINMENU);
    }

    public void QuitButtonClicked() {
        AudioManager.instance.playSoundEvent("ButtonPress");
        Application.Quit();
    }
}
