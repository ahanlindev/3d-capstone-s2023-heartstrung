using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance {get; private set;}

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start() {
        // Don't destroy this between scenes
        DontDestroyOnLoad(this.gameObject);
    }
}
