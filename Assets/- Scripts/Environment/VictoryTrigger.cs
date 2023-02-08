using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class VictoryTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) {
            Debug.Log("Collided with " + collider.gameObject.name);
            if(collider.gameObject.CompareTag("Player")) {
                Debug.Log("Victory!");
                int index = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadSceneAsync(index + 1);
            }
    }
}
