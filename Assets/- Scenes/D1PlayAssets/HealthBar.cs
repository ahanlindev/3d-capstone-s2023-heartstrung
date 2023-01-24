using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public BattleManager battleManager;


    private void OnEnable()
    {
        battleManager.kittyTakeDmgEvent += SetHealth;
    }

    private void OnDisable()
    {
        battleManager.kittyTakeDmgEvent -= SetHealth;
    }


    public void SetHealth(int health)
    {
        Debug.Log("called");
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}