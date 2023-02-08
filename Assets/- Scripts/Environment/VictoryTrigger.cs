using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class VictoryTrigger : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
            Debug.Log("Collided with " + collision.gameObject.name);
            if(collision.gameObject.CompareTag("Player")) {
                Debug.Log("Victory!");
                int index = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadSceneAsync(index + 1);
            }
    }
}
