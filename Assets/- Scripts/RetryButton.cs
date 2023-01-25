using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    PlayerInput playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
    }
    void OnEnable() {
        playerInput.Gameplay.Jump.performed += Retry;
    }

    void OnDisable() {
        playerInput.Gameplay.Jump.performed -= Retry;
    }

    // Update is called once per frame
    void Retry(InputAction.CallbackContext ctx) {
        SceneManager.LoadSceneAsync(0);
    }
}
