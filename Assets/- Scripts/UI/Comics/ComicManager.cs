using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>Controls displaying and hiding comic pages in sequence</summary>
public class ComicManager : MonoBehaviour
{
    [Tooltip("Time in seconds that each new panel will fade in")]
    [SerializeField] private float _fadeInTime = 0.25f;

    [Tooltip("Time in seconds that each new panel will fade out")]
    [SerializeField] private float _fadeOutTime = 0.25f;

    private int _index = 0;

    private TweenParams _fadeParams;

    void Start() {
        // set all child canvases to be inactive
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
            child.GetComponent<RawImage>().material.color = Color.white;
        }

        // initialize fade parameters
        _fadeParams = new TweenParams()
            .SetEase(Ease.InOutCubic);

        // activate the starting index
        FadeCurrentSlide(fadeIn: true);
        //transform.GetChild(_index).gameObject.SetActive(true);
    }

    private void OnDestroy() {
        DOTween.KillAll();    
    }
    
    public void Next() {
        if (_index >= transform.childCount - 1)
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadSceneAsync(index + 1);
        } else {
            //transform.GetChild(_index).gameObject.SetActive(false);
            //FadeCurrentSlide(fadeIn: false);
            _index++;
            // transform.GetChild(_index).gameObject.SetActive(true);
            FadeCurrentSlide(fadeIn: true);
        }
    }

    public void Previous() {
        // if(_index > 0) {
        //     transform.GetChild(_index).gameObject.SetActive(false);
        //     _index--;
        //     transform.GetChild(_index).gameObject.SetActive(true);
        // }
        if(_index > 0) {
            FadeCurrentSlide(fadeIn: false);
            _index--;
            //FadeCurrentSlide(fadeIn: true);
        }
    }

    public void Skip() {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(index + 1);
    }

    private void FadeCurrentSlide(bool fadeIn)
    {
        // set up variables
        Debug.Log($"index {_index}");
        RawImage image = transform.GetChild(_index)?.GetComponent<RawImage>();
        float startAlpha = (fadeIn) ? 0.0f : 1.0f;
        float endAlpha = (fadeIn) ? 1.0f : 0.0f;
        float fadeTime = (fadeIn) ? _fadeInTime : _fadeOutTime;

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
