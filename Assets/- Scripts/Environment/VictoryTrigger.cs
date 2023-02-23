using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class VictoryTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
            if(collider.gameObject.CompareTag("Player")) {
                TransitionManager.TransitionToNextScene();
            }
    }
}
