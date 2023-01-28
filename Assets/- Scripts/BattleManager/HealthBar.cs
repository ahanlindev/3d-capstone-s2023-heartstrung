using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;


    private void OnEnable()
    {
        BattleManager.kittyTakeDmgEvent += SetHealth;
    }

    private void OnDisable()
    {
        BattleManager.kittyTakeDmgEvent -= SetHealth;
    }


    public void SetHealth(int health)
    {
        Debug.Log("called");
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}