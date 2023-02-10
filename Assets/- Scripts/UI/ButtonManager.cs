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
        Debug.Log("Tutorial GO!");
        SceneManager.LoadSceneAsync(1);
    }

    public void OptionsButtonClicked() {
        Debug.Log("Options GO!");
        OptionsMenuManager.instance.ChangeOptionsState();
    }

    public void UnpauseButtonClicked() {
        Debug.Log("Unpaused");
        PauseMenuManager.instance.Unpause();
    }

    public void MainMenuButtonClicked() {
        Debug.Log("Main Menu GO!");
        PauseMenuManager.instance.Unpause();
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void QuitButtonClicked() {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
