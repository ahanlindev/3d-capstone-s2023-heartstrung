using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
            Debug.Log("Collided with " + collision.gameObject.name);
            if(collision.gameObject.name == "Kitty") {
                Debug.Log("Victory!");
                SceneManager.LoadSceneAsync("VictoryScene");
            }
    }
}
