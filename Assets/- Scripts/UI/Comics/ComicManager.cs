using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ComicManager : MonoBehaviour
{
    public Canvas[] slideShow;
    public int index = 0;
    public Scene nextScene;

    void Start() {
        // set all child canvases to be inactive
        foreach(Transform child in transform) {
            if(child.gameObject.name != "Button" && child.gameObject.name != "Background") {
                child.gameObject.SetActive(false);
            }
        }

        // activate the starting index
        transform.GetChild(index).gameObject.SetActive(true);
    }

    public void Next() {
        if(index >= slideShow.Length - 1) {
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadSceneAsync(index + 1);
        } else {
            transform.GetChild(index).gameObject.SetActive(false);
            var image = transform.GetChild(index).gameObject;
            // image.GetComponent<Image>().DOColor(new Color(0,0,0), 0.25f);
            index++;
            transform.GetChild(index).gameObject.SetActive(true);
        }
    }

    public void Previous() {
        if(index > 0) {
            transform.GetChild(index).gameObject.SetActive(false);
            index--;
            transform.GetChild(index).gameObject.SetActive(true);
        }
    }

    public void Skip() {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(index + 1);
    }
}