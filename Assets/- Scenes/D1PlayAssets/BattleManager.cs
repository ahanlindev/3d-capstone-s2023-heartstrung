using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 100;
    public int HPHurtEveryTime = 5;
    public static BattleManager Instance { get; private set; }
    public HealthBar playerHealthBar;


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
        playerHealthBar.SetHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            substractHP();
        }
    }

    void substractHP()
    {
        health -= HPHurtEveryTime;
        playerHealthBar.SetHealth(health);
    }
}
