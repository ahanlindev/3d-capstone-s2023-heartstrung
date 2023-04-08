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
    public float maxHealth => _maxHealth;

    /// <summary>current health value</summary>
    public float currentHealth { get; private set; }


    private void Awake()
    {
        currentHealth = _maxHealth;        
    }
    
    /// <summary>
    /// Updates the current amount of health by the specified 
    /// value (clamped to max health), and sends a corresponding event. 
    /// </summary>
    public void ChangeHealth(float delta) {
        currentHealth += delta;

        // impose upper limit on health
        currentHealth = Mathf.Clamp(currentHealth, currentHealth, _maxHealth);
        ChangeHealthEvent?.Invoke(currentHealth, delta);
    }
}
