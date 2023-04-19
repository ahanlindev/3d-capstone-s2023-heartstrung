using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeButton : MonoBehaviour
{

    /// <summary>color that the button is when the game begins. Used in tweening.</summary>
    private Color _originalColor;
    private Renderer _buttonBodyRenderer;
    // Start is called before the first frame update
    private void Start()
    {
        _buttonBodyRenderer = GetComponent<Renderer>();
        _originalColor = _buttonBodyRenderer.material.color;

        // fade color 
        Color newColor = _originalColor * .6f;
        _buttonBodyRenderer.material.color = newColor;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
