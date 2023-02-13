using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>Controls displaying and hiding comic pages in sequence</summary>
public class ComicManager : MonoBehaviour
{
    // TODO might be more efficient to have many images one one canvas
    [Tooltip("Array of comic panels that will be shown in order")]
    [SerializeField] private Canvas[] _slideShow;

    [Tooltip("Time in seconds that each new panel will fade in")]
    [SerializeField] private float _fadeInTime = 0.25f;

    [Tooltip("Time in seconds that each new panel will fade out")]
    [SerializeField] private float _fadeOutTime = 0.25f;

    private int _index = 0;

    private TweenParams _fadeParams;

    void Start() {
        // set all child canvases to be inactive
        foreach(Transform child in transform) {
            if(child.gameObject.name != "Button" && child.gameObject.name != "Background") {
                child.gameObject.SetActive(false);
            }
        }

        // initialize fade parameters
        _fadeParams = new TweenParams()
            .SetEase(Ease.InOutCubic);

        // activate the starting index
        transform.GetChild(_index).gameObject.SetActive(true);
    }

    private void OnDestroy() {
        DOTween.KillAll();    
    }
    
    public void Next() {
        if (_index >= _slideShow.Length - 1)
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadSceneAsync(index + 1);
        } else {
            transform.GetChild(_index).gameObject.SetActive(false);
            var image = transform.GetChild(_index).gameObject;
            // image.GetComponent<Image>().DOColor(new Color(0,0,0), 0.25f);
            _index++;
            transform.GetChild(_index).gameObject.SetActive(true);
        }
    }

    public void Previous() {
        if(_index > 0) {
            transform.GetChild(_index).gameObject.SetActive(false);
            _index--;
            transform.GetChild(_index).gameObject.SetActive(true);
        }
    }

    public void Skip() {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(index + 1);
    }

    private void FadeCurrentSlide(bool fadeIn)
    {
        // set up variables
        Canvas canvas = _slideShow[_index];
        Image[] images = canvas.GetComponentsInChildren<Image>();
        float startAlpha = (fadeIn) ? 0.0f : 1.0f;
        float endAlpha = (fadeIn) ? 1.0f : 0.0f;
        float fadeTime = (fadeIn) ? _fadeInTime : _fadeOutTime;
        
        // fade in or fade out each image as appropriate
        foreach (Image image in images)
        {
            // start tween sequence
            Sequence fade = DOTween.Sequence()
                .Append(image.material.DOFade(startAlpha, 0))
                .Append(image.material.DOFade(endAlpha, fadeTime))
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
}
