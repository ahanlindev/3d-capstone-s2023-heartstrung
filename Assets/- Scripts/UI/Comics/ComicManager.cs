using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>Controls displaying and hiding comic pages in sequence</summary>
public class ComicManager : MonoBehaviour
{
    [Tooltip("Time in seconds that each new panel will fade in")]
    [SerializeField] private float _panelFadeInTime = 0.5f;

    [Tooltip("Time in seconds that each new panel will fade out")]
    [SerializeField] private float _panelFadeOutTime = 0.25f;

    [Tooltip("Time in seconds that the player must hold the skip button")]
    [SerializeField] private float _skipHoldTime = 1.0f;

    [Tooltip("Are we transitioning to the next scene?")]
    [SerializeField] private bool _transitioning = false;
    

    /// <summary>Index of current panel</summary>
    private int _index = 0;

    /// <summary>Standard parameters for tweens to fade panels in and out </summary>
    private TweenParams _fadeParams;

    /// <summary>Input bindings for controller/keyboard comic control</summary>
    private PlayerInput _input;

    /// <summary>Id string that allows fade tweens to be tracked and controlled</summary>
    private readonly string _fadeId = "ComicManagerFade";

    #region SETUP_TEARDOWN

    void Start()
    {
        // initialize input and input events after scene fades in
        _input = new PlayerInput();
        TransitionManager.FadeinFinishEvent += SetupInput;

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

        // clean up event subscriptions
        TransitionManager.FadeinFinishEvent -= SetupInput;

        // teardown input and input events
        TeardownInput();
    }

    #endregion
    #region INPUT_HANDLERS

    private void SetupInput() {
        _input.Comic.Next.performed += OnNextInput;
        _input.Comic.Prev.performed += OnPrevInput;
        _input.Comic.Skip.performed += OnSkipStartInput;
        _input.Comic.Skip.canceled += OnSkipCancelInput;

        PauseMenuManager.PauseEvent += _input.Disable;        
        PauseMenuManager.UnpauseEvent += _input.Enable; 

        _input.Enable(); 
    }

    private void TeardownInput() {
        _input.Comic.Next.performed -= OnNextInput;
        _input.Comic.Prev.performed -= OnPrevInput;
        _input.Comic.Skip.performed -= OnSkipStartInput;
        _input.Comic.Skip.canceled -= OnSkipCancelInput;

        PauseMenuManager.PauseEvent -= _input.Disable;        
        PauseMenuManager.UnpauseEvent -= _input.Enable;

        _input.Disable();
    }
    private void OnNextInput(CallbackContext _) {
        if(!PauseMenuManager.instance.paused) {
           Next(); 
        }
    }
    private void OnPrevInput(CallbackContext _) {
        if(!PauseMenuManager.instance.paused) {
           Previous(); 
        }
    }

    private void OnSkipStartInput(CallbackContext _) {
        if(PauseMenuManager.instance.paused) {
            TransitionManager.TransitionToNextScene(_skipHoldTime);
        }
    }

    private void OnSkipCancelInput(CallbackContext _) {
        TransitionManager.CancelTransition();
    }

    #endregion

    /// <summary>
    /// Continue to the next page of the comic. 
    /// If the last page is reached, loads the next scene.
    /// </summary>
    public void Next()
    {
        if(_transitioning) {
            return;
        }
        AudioManager.instance.playSoundEvent("ComicAdvance");
        if (_index >= transform.childCount - 1)
        {
            TransitionManager.TransitionToNextScene();
            _transitioning = true;
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
        if(_transitioning) {
            return;
        }
        if (_index > 0)
        {
            AudioManager.instance.playSoundEvent("ComicAdvance");
            FadeCurrentPanel(fadeIn: false);
            _index--;
        }
    }

    /// <summary>Skip the comic and transitions to the next scene/</summary>
    public void Skip() {
        if(_transitioning) {
            return;
        }
        AudioManager.instance.playSoundEvent("ComicAdvance");
        TransitionManager.TransitionToNextScene();
        _transitioning = true;
    }

    /// <summary>Gradually fade the comic panel at the current index either in or out</summary>
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
}
