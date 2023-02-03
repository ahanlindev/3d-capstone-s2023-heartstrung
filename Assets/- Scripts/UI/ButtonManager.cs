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
        SceneManager.LoadSceneAsync("TutorialLevel");
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
}
