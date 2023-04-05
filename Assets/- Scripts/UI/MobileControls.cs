using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileControls : MonoBehaviour
{
    [SerializeField] private GameObject _joystick;
    [SerializeField] private GameObject _actionButtons;
    [SerializeField] private GameObject _pauseButton;
    
    [Tooltip("Scenes that will display mobile control UI when active and played on a mobile device")]
    [SerializeField] private SceneID[] _sceneWhitelist;

    private void Start()
    {
        // this functionality should only exist if on a mobile device
        // if (!Application.isMobilePlatform)
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        // initialize on-screen control UI
        SceneManager.sceneLoaded += OnSceneLoaded;
        ToggleUIByScene(SceneID.MAINMENU);
    }

    private void OnDestroy() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        ToggleUIByScene(scene.ToSceneID());
    }
    
    private void ToggleUIByScene(SceneID scene)
    {
        // display controls if the scene is whitelisted
        bool displayControlUI = _sceneWhitelist.Contains(scene);
        _joystick.SetActive(displayControlUI);
        _actionButtons.SetActive(displayControlUI);

        // display pause button if able to pause
        bool canPause = PauseMenuManager.instance.CanPause(); 
        Debug.Log("Can Pause? " + canPause);
        _pauseButton.SetActive(canPause);
    }
}
