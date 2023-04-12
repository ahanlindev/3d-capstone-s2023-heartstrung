using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

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

    [Tooltip("Object reference to Prev Button")]
    [SerializeField] private GameObject prevButton;
    [Tooltip("Object reference to Next Button")]
    [SerializeField] private GameObject nextButton;
    [Tooltip("Object reference to Skip Button")]
    [SerializeField] private GameObject skipButton;
    
    [Tooltip("If true, the Prev Button will be disabled when on the first panel")]
    [SerializeField] private bool _hidePrevAtStart = true;

    [Tooltip("If true, the Next Button will be disabled when on the last panel")]
    [SerializeField] private bool _hideNextAtEnd = false;
    
    [Tooltip("If true, the player will not be able to skip")]
    [SerializeField] private bool _disableSkip = false;

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
        TransitionManager.FadeoutFinishEvent += HideEverything;
        
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
        
        // Initialize button state
        UpdateButtons();
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
        Next(); 
    }
    private void OnPrevInput(CallbackContext _) {
        Previous(); 
    }

    private void OnSkipStartInput(CallbackContext _) {
        TransitionManager.TransitionToNextScene(_skipHoldTime);
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
        // input ignored in these states
        if(_transitioning || PauseMenuManager.instance.paused) {
            return;
        }
        
        bool atLastIdx = _index >= transform.childCount - 1;
        if (_hideNextAtEnd && atLastIdx) { return; }
        
        // validated. Actually advance page
        AudioManager.instance.playSoundEvent("ComicAdvance");
        if (atLastIdx)
        {
            TransitionManager.TransitionToNextScene();
            _transitioning = true;
        }
        else
        {
            _index++;
            FadeCurrentPanel(fadeIn: true);
        }
       
        // Update button state
        UpdateButtons();
    }


    /// <summary>Return to the previous page of the comic, if it exists/</summary>
    public void Previous()
    {
        if(_transitioning || PauseMenuManager.instance.paused) {
            return;
        }
        
        if (_index > 0)
        {
            AudioManager.instance.playSoundEvent("ComicAdvance");
            FadeCurrentPanel(fadeIn: false);
            _index--;
        }
        
        // Update button state
        UpdateButtons();
    }

    /// <summary>Skip the comic and transitions to the next scene/</summary>
    public void Skip() {
        if(_transitioning || PauseMenuManager.instance.paused) {
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
        // set up variables for fade
        RawImage image = transform.GetChild(_index)?.GetComponent<RawImage>();
        float startAlpha = (fadeIn) ? 0.0f : 1.0f;
        float endAlpha = (fadeIn) ? 1.0f : 0.0f;
        float fadeTime = (fadeIn) ? _panelFadeInTime : _panelFadeOutTime;

        
        // fade in or fade out image as appropriate
        Color startColor = image.color;
        startColor.a = startAlpha;
        image.color = startColor;

        // start tween sequence
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
    
    /// <summary>
    /// Hides and shows the next and prev buttons as appropriate based on state
    /// </summary>
    /// <param name="hideAll">If true, overrides normal settings and hides all buttons</param>
    private void UpdateButtons(bool hideAll = false)
    {
        bool atLastIdx = _index == transform.childCount - 1;
        
        // initialize conditions
        bool showPrev = !(_hidePrevAtStart && _index == 0) && !hideAll;
        bool showNext = !(_hideNextAtEnd && atLastIdx) && !hideAll;
        bool showSkip = !(_disableSkip) && !atLastIdx && !hideAll;
        
        // handle button visuals
        if (prevButton) { prevButton.SetActive(showPrev); }
        if (nextButton) { nextButton.SetActive(showNext); }
        if (skipButton) { skipButton.SetActive(showSkip); }
        
        // handle input bindings
        if (showPrev) {  _input.Comic.Prev.Enable(); }
        else { _input.Comic.Prev.Disable(); }
        
        if (showNext) {  _input.Comic.Next.Enable(); }
        else { _input.Comic.Next.Disable(); }

        if (showSkip) { _input.Comic.Skip.Enable(); }
        else { _input.Comic.Skip.Disable(); }
    }

    /// <summary>
    /// Hide all panels and disable all buttons. Used when leaving the scene.
    /// </summary>
    private void HideEverything()
    {
        // Hide Buttons
        UpdateButtons(hideAll: true);
        
        // Hide panels
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
