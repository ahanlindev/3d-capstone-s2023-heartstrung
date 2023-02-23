using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatTrigger : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
            Debug.Log("Collided with " + collision.gameObject.name);
            if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Heart") ) {
                Debug.Log("Defeat!");
                TransitionManager.TransitionToScene(SceneID.GAME_OVER);
            }
    }
}
