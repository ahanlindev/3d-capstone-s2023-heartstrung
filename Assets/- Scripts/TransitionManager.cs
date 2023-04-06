using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TransitionManager : MonoBehaviour {
    
    #region EVENTS

    
    /// <summary>Event emitted when leaving a scene and screen has fully faded out</summary>
    public static event Action FadeoutFinishEvent;
    
    /// <summary>Event emitted when entering a scene and screen has fully faded in</summary>
    public static event Action FadeinFinishEvent;
    
    #endregion

    #region FIELDS

    private static TransitionManager _instance;
    
    private RawImage _screenFadeOverlay;
    private Tween _fadeoutTween;
    
    [Tooltip("Time that the screen will take to fade in/out between scenes")]
    [SerializeField] private float _fadeTime;
    
    [Tooltip("Color that the screen will fade to when transitioning between scenes")]
    [SerializeField] private Color _fadeoutColor = Color.black;


    #endregion

    #region SETUP_TEARDOWN

    private void Awake() {
        if (!_instance) {
            _instance = this;
        } else {
            Destroy(this);
            return;
        }

        _screenFadeOverlay = GetComponentInChildren<RawImage>();
        if (!_screenFadeOverlay) { Debug.LogError("TransitionManager cannot find screen fade overlay image"); }
        _screenFadeOverlay.DOFade(0,0); // start transparent
    }

    #endregion

    #region PUBLIC_METHODS

    /// <summary>Transition to the current scene, restarting it.</summary>
    /// <param name="fadeTimeOverride">
    /// If nonzero, overrides normal fadeout time of the scene transition
    /// </param>
    public static void ResetScene(int fadeTimeOverride = 0)
    {
        if (!_instance) { return; }
        int idx = SceneManager.GetActiveScene().buildIndex;
        _instance.DoTransitionToScene(
            SceneManager.GetSceneByBuildIndex(idx).name, 
            fadeTimeOverride
        );
    }

    /// <summary>
    /// Transition to the next scene in the build order. 
    /// Loops back around to 0 if the current
    ///  scene is the last in the build order.
    /// </summary>
    /// <param name="fadeTimeOverride">
    /// If nonzero, overrides normal fadeout time of the scene transition
    /// </param>
    public static void TransitionToNextScene(float fadeTimeOverride = 0)
    {
        if (!_instance) { return; }
        
        int idx = SceneManager.GetActiveScene().buildIndex + 1;
        idx = idx % SceneManager.sceneCountInBuildSettings;
        string sceneName = SceneUtility.GetScenePathByBuildIndex(idx);
        
        _instance.DoTransitionToScene(sceneName, fadeTimeOverride);
    } 
    
    /// <summary>Transition to the scene specified by the enum value</summary>
    /// <param name="sceneID">The id value of the desired scene</param>
    /// <param name="fadeTimeOverride">
    /// If nonzero, overrides normal fadeout time of the scene transition
    /// </param>
    public static void TransitionToScene(SceneID sceneID, float fadeTimeOverride = 0)
    {
        if (!_instance) { return; }
        _instance.DoTransitionToScene(sceneID.GetName(), fadeTimeOverride);
    }

    /// <summary>Cancel any current scene transition and reverses screen fadeout.</summary>
    public static void CancelTransition() {
        if (!_instance) { return; }
        Tween fadeout = _instance._fadeoutTween; 
        if (fadeout != null && fadeout.active) {
            // reverse screen fade and remove any callbacks
            fadeout.OnComplete(() => {})
                .Flip();
        }
    }

    #endregion

    #region PRIVATE_METHODS

    /// <summary>Fade the screen out and transition scenes, then fade back in.</summary>
    /// <param name="sceneName">the string name of the specified scene</param>
    /// <param name="fadeTimeOverride">
    /// If nonzero, overrides normal fadeout time of the scene transition
    /// </param>
    private void DoTransitionToScene(string sceneName, float fadeTimeOverride = 0) {
        // handle param
        float fadeTime =(fadeTimeOverride == 0) ? _fadeTime : fadeTimeOverride;

        // kill current fade tween if active.
        if (_fadeoutTween != null && _fadeoutTween.active) {
            _fadeoutTween.Rewind();
            _fadeoutTween.Kill();
        }

        // set overlay to correct color and make it transparent
        _screenFadeOverlay.color = _fadeoutColor - new Color(0,0,0,1);


        _fadeoutTween = DOTween.Sequence()
            // fade out
            .Append(_screenFadeOverlay.DOFade(endValue: 1, duration: fadeTime)
                .OnComplete(() =>
                {
                    SceneManager.LoadSceneAsync(sceneName);
                    FadeoutFinishEvent?.Invoke();
                }))
            
            // wait a little bit
            .AppendInterval(0.5f)

            // fade back in
            .Append(_screenFadeOverlay.DOFade(0, fadeTime))
                .OnComplete(() =>FadeinFinishEvent?.Invoke());
    }

    #endregion
}