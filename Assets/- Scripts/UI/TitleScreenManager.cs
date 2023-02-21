using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance {get; private set;}
    public GameObject titleScreen;

    [Tooltip("First button that will be selected when this menu is enabled.")]
    [SerializeField] private UnityEngine.UI.Button defaultSelection;

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }
    
    void Start() {
        EnableMenu();
    }

    public void DisableMenu() {
        titleScreen.SetActive(false);
    }

    public void EnableMenu() {
        titleScreen.SetActive(true);
        if (!defaultSelection) { Debug.LogError("Title menu has no default button selected!"); }
        defaultSelection.Select();
    }
}
