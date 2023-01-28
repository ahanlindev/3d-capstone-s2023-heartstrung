using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update    

    public int health = 100;
    public int HPHurtEveryTime = 34;

    public static BattleManager Instance { get; private set; }

    public static event Action<int> kittyTakeDmgEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    
    public void KittyTakeDmg() { 
        health -= HPHurtEveryTime; 
        kittyTakeDmgEvent?.Invoke(health); 
        if(health <= 0) {
            SceneManager.LoadSceneAsync("DefeatScene");
        }
    }
}