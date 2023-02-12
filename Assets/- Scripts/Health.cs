using System;
using UnityEngine;

/// <summary>Measures and updates health. Notifies listeners when health changes.</summary>
public class Health : MonoBehaviour
{
    // Emitted events
    /// <summary> 
    /// Emitted when current health changes. 
    /// First parameter is the new amount. Second parameter is the delta 
    /// </summary>
    public event Action<float, float> ChangeHealthEvent;
    
    [Tooltip("Max health for this entity")]
    [SerializeField] private float _maxHealth = 100f;
    public float MaxHealth {get => _maxHealth; private set => _maxHealth = value; }

    /// <summary>current health value</summary>
    public float CurrentHealth { get; private set; }


    void Awake()
    {
        CurrentHealth = _maxHealth;        
    }

    /// <summary>
    /// Updates the current amount of health by the specified 
    /// value, and sends a corresponding event. 
    /// </summary>
    public void ChangeHealth(float delta) {
        CurrentHealth += delta;

        // impose upper limit on health
        CurrentHealth = Mathf.Clamp(CurrentHealth, CurrentHealth, _maxHealth);
        ChangeHealthEvent?.Invoke(CurrentHealth, delta);
    }
}
