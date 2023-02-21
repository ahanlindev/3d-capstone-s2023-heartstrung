using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>Controls displaying and hiding comic pages in sequence</summary>
public class ComicManager : MonoBehaviour
{
    /// TODO: replace with version in scene management class
    [Tooltip("Image used to cover the screen when fading out of the scene. ")]
    [SerializeField] private RawImage _fadeOutOverlay;

    [Tooltip("Time in seconds that each new panel will fade in")]
    [SerializeField] private float _panelFadeInTime = 0.5f;

    [Tooltip("Time in seconds that each new panel will fade out")]
    [SerializeField] private float _panelFadeOutTime = 0.25f;

    [Tooltip("Time in seconds that the player must hold the skip button")]
    [SerializeField] private float _skipHoldTime = 1.0f;
    

    /// <summary>Index of current panel</summary>
    private int _index = 0;

    /// <summary>Standard parameters for tweens to fade panels in and out </summary>
    private TweenParams _fadeParams;

    /// <summary>Input bindings for controller/keyboard comic control</summary>
    private PlayerInput _input;

    /// <summary>Tween used to measure how long the player has been holding skip</summary>
    private Tween _skipTimer;

    /// <summary>Id string that allows fade tweens to be tracked and controlled</summary>
    private readonly string _fadeId = "ComicManagerFade";

    void Start()
    {
        // initialize input and input events
        _input = new PlayerInput();
        _input.Enable();

        _input.Comic.Next.performed += OnNextInput;
        _input.Comic.Prev.performed += OnPrevInput;
        _input.Comic.Skip.performed += OnSkipStartInput;
        _input.Comic.Skip.canceled += OnSkipCancelInput;

        PauseMenuManager.PauseEvent += _input.Disable;        
        PauseMenuManager.UnpauseEvent += _input.Enable;        

        // ensure fade out image exists, then make it transparent
        if (!_fadeOutOverlay) { Debug.LogError("fadeOutOverlay is not set in ComicManager!"); }
        _fadeOutOverlay?.DOFade(endValue: 0f, duration: 0f);

        // set all child canvases to be inactive
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // initialize panel fade parameters
        _fadeParams = new TweenParams()
            .SetEase(Ease.InOutCubic)
            .SetId(_fadeId);

        // activate the starting index
        FadeCurrentPanel(fadeIn: true);
    }

    private void OnDestroy()
    {
        // cancel all in-progress tweens if relevant
        DOTween.Kill(_fadeId);

        // teardown input and input events
        _input.Comic.Next.performed -= OnNextInput;
        _input.Comic.Prev.performed -= OnPrevInput;

        _input.Disable();
    }
    
    // Input handlers
    private void OnNextInput(CallbackContext _) => Next();
    private void OnPrevInput(CallbackContext _) => Previous();

    private void OnSkipStartInput(CallbackContext _) {
        _skipTimer?.Kill(); // clean up existing tweens
        _skipTimer = _fadeOutOverlay.DOFade(endValue: 1, duration: _skipHoldTime)
            .SetAs(_fadeParams)
            .OnComplete(LoadNextScene);
    }

    private void OnSkipCancelInput(CallbackContext _) {
        _skipTimer?.Flip(); // fade back in 
        _skipTimer?.OnComplete(() => {}); // cancels the scene load
    }

    /// <summary>
    /// Continues to the next page of the comic. 
    /// If the last page is reached, loads the next scene.
    /// </summary>
    public void Next()
    {
        if (_index >= transform.childCount - 1)
        {
            // TODO really want a scene-control singleton for this stuff
            _fadeOutOverlay.DOFade(endValue: 1, duration: _panelFadeInTime)
                .SetAs(_fadeParams)
                .OnComplete(LoadNextScene);
        }
        else
        {
            _index++;
            FadeCurrentPanel(fadeIn: true);
        }
    }


    /// <summary>Return to the previous page of the comic, if it exists/</summary>
    public void Previous()
    {
        if (_index > 0)
        {
            FadeCurrentPanel(fadeIn: false);
            _index--;
        }
    }

    /// <summary>Skips the comic and loads the next scene</summary>
    public void Skip()
    {
        LoadNextScene();
    }

    /// <summary>Gradually fades the comic panel at the current index either in or out</summary>
    /// <param name="fadeIn">If true, the comic panel at the current index is enabled, otherwise it is disabled.</param>
    private void FadeCurrentPanel(bool fadeIn)
    {
        // set up variables
        Debug.Log($"index {_index}");
        RawImage image = transform.GetChild(_index)?.GetComponent<RawImage>();
        float startAlpha = (fadeIn) ? 0.0f : 1.0f;
        float endAlpha = (fadeIn) ? 1.0f : 0.0f;
        float fadeTime = (fadeIn) ? _panelFadeInTime : _panelFadeOutTime;

        // fade in or fade out image as appropriate
        Color startColor = image.color;
        startColor.a = startAlpha;
        image.color = startColor;

        // start tween sequence
        Debug.Log($"fadeInTime {fadeTime}");
        Tween fade = image.DOFade(endAlpha, fadeTime)
            .SetAs(_fadeParams)
        ;

        if (fadeIn)
        {
            // activate game object at start of fade in
            fade.OnStart(() => { image.gameObject.SetActive(true); });
        }
        else
        {
            // deactivate game object at end of fade out
            fade.OnComplete(() => { image.gameObject.SetActive(false); });
        }
    }

    /// <summary>Fades out all active panels and loads the next scene</summary>
    // TODO replace with SceneManager
    private void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(index + 1);
    }
}
