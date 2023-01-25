using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update    

    public int health = 100;
    public int HPHurtEveryTime = 5;

    public static BattleManager Instance { get; private set; }

    public event Action<int> kittyTakeDmgEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        kittyTakeDmgEvent?.Invoke(health);

    }

    private void OnKittyTakeDmg() { 
        health -= HPHurtEveryTime; 
        if(health <= 0) {
            SceneManager.LoadSceneAsync("DefeatScene");
        }
        kittyTakeDmgEvent?.Invoke(health); 
    }
}