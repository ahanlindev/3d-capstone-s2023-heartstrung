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
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        TransitionManager.TransitionToScene(SceneID.COMIC1);
    }

    public void OptionsButtonClicked() {
        // Debug.Log("Options GO!");
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        OptionsMenuManager.instance.ChangeOptionsState();
    }

    public void ControlsButtonClicked() {
        // Debug.Log("Controls GO!");
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        ControlsMenuManager.instance.ChangeControlsState();
    }

    public void UnpauseButtonClicked() {
        // Debug.Log("Unpaused");
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        PauseMenuManager.instance.Unpause();
    }

    public void ResetButtonClicked() {
        // Debug.Log("Reset");
        CheckpointManager manager = FindObjectsOfType<CheckpointManager>()[0];
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        manager.ResetToCheckpoint();
        PauseMenuManager.instance.Unpause();
    }

    public void LevelSelectButtonClicked() {
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        TransitionManager.TransitionToScene(SceneID.LEVELSELECT);
    }

    public void LevelButtonClicked(SceneID levelID) {
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        TransitionManager.TransitionToScene(levelID);
    }

    public void MainMenuButtonClicked() {
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        PauseMenuManager.instance.Unpause();
        TransitionManager.TransitionToScene(SceneID.MAINMENU);
    }

    public void QuitButtonClicked() {
        AudioManager.instance.PlaySoundEvent("UIButtonPress");
        Application.Quit();
    }
}
